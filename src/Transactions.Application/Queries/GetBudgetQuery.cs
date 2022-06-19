using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Transactions.Application.Constants;
using Transactions.Application.Exceptions;
using Transactions.Application.Interfaces;
using Transactions.Application.Models;

namespace Transactions.Application.Queries
{
    public class GetBudgetQuery : IRequest<BudgetViewModel>
    {
        private int BudgetId { get; set; }
        public GetBudgetQuery(int budgetId)
        {
            BudgetId = budgetId;
        }
        public class Handler : IRequestHandler<GetBudgetQuery, BudgetViewModel>
        {
            private readonly IFinancialService _financialService;
            private readonly IAccessTokenService _accessTokenService;
            private readonly IBudgetService _budgetService;
            public Handler(IFinancialService financialService, IAccessTokenService accessTokenService, IBudgetService budgetService)
            {
                _financialService = financialService;
                _accessTokenService = accessTokenService;
                _budgetService = budgetService;
            }

            public async Task<BudgetViewModel> Handle(GetBudgetQuery request, CancellationToken cancellationToken)
            {
                var allTransactions = new List<TransactionModel>();
                var expiredAccessTokens = new List<ExpiredAccessItem>();
                var budget = await _budgetService.GetBudgetAsync(request.BudgetId);
                var accessTokens = await _accessTokenService.GetBudgetAccessTokensAsync(budget.Id);
                foreach(var accessToken in accessTokens)
                {
                    try
                    {
                        var transactions = await _financialService.GetTransactionsAsync(accessToken.AccessToken, budget.StartDate, budget.EndDate);
                        allTransactions.AddRange(transactions);
                    }
                    catch (FinancialServiceException fex)
                    {
                        if (string.Equals(fex.Error?.error_code, ErrorCodes.ITEM_LOGIN_REQUIRED,
                                   StringComparison.InvariantCultureIgnoreCase))
                        {
                            var institution = await _financialService.GetInstitutionAsync(accessToken.InstitutionId);
                            expiredAccessTokens.Add(new ExpiredAccessItem(accessToken.AccessToken, fex.Error.error_message, institution));
                        }
                    }
                }

                return new BudgetViewModel(allTransactions, budget.StartDate, budget.EndDate, expiredAccessTokens);
            }
        }

    }
}
