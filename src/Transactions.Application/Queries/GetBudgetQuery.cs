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
            private readonly ICategoryService _categoryService;
            public Handler(IFinancialService financialService, IAccessTokenService accessTokenService,
                IBudgetService budgetService, ICategoryService categoryService)
            {
                _financialService = financialService;
                _accessTokenService = accessTokenService;
                _budgetService = budgetService;
                _categoryService = categoryService;
            }

            public async Task<BudgetViewModel> Handle(GetBudgetQuery request, CancellationToken cancellationToken)
            {
                var budget = await _budgetService.GetBudgetAsync(request.BudgetId);
                var accessTokens = await _accessTokenService.GetBudgetAccessTokensAsync(budget.Id);

                var expiredAccessTokens = new List<ExpiredAccessItem>();
                var budgetAccessItems = new List<UserAccessItemModel>();
                var allTransactions = new List<TransactionModel>();
                foreach (var accessToken in accessTokens)
                {
                    var institution = await _financialService.GetInstitutionAsync(accessToken.InstitutionId);
                    try
                    {
                        var accounts = await _financialService.GetAccountsAsync(accessToken.AccessToken);
                        var item = await _financialService.GetItemAsync(accessToken.AccessToken);
                        
                        budgetAccessItems.Add(new UserAccessItemModel
                        {
                            Accounts = accounts,
                            Institution = institution,
                            Item = item,
                            UserAccessItemId = accessToken.Id
                        });

                        var transactions = await _financialService.GetTransactionsAsync(accessToken.AccessToken, budget.StartDate, budget.EndDate);
                        
                        allTransactions.AddRange(transactions);
                    }
                    catch (FinancialServiceException fex)
                    {
                        if (string.Equals(fex.Error?.error_code, ErrorCodes.ITEM_LOGIN_REQUIRED,
                                   StringComparison.InvariantCultureIgnoreCase))
                        {
                            expiredAccessTokens.Add(new ExpiredAccessItem(accessToken.AccessToken, fex.Error.error_message, institution));
                        }
                    }
                }

                var excludedTransactions = await _budgetService.GetExcludedTransactionsAsync(budget.Id);
                var budgetCategories = await _budgetService.GetBudgetCategoriesAsync(request.BudgetId);
                var transactionCategories = await _categoryService.GetTransactionCategoriesAsync(request.BudgetId);

                return new BudgetViewModel(budget.Name, budget.StartDate, budget.EndDate, 
                    budgetCategories, transactionCategories, allTransactions, budgetAccessItems, 
                    expiredAccessTokens, excludedTransactions);
            }
        }

    }
}
