using MediatR;
using Microsoft.Extensions.Logging;
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
    public class GetTransactionsQuery : IRequest<GetTransactionsQuery.Response>
    {
        private int BudgetId { get; set; }
        public GetTransactionsQuery(int budgetId)
        {
            BudgetId = budgetId;
        }

        public class Handler : IRequestHandler<GetTransactionsQuery, Response>
        {
            private readonly IFinancialService _financialService;
            private readonly IBudgetService _budgetService;
            private readonly IAccessTokenService _accessTokenService;
            private readonly ICategoryService _categoryService;

            public Handler(IFinancialService financialService, IAccessTokenService accessTokenService, 
                IBudgetService budgetService, ICategoryService categoryService)
            {
                _financialService = financialService;
                _accessTokenService = accessTokenService;
                _budgetService = budgetService;
                _categoryService = categoryService;
            }

            public async Task<Response> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
            {
                var allTransactions = new List<TransactionModel>();
                var expiredAccessItems = new List<ExpiredAccessItem>();
                var budget = await _budgetService.GetBudgetAsync(request.BudgetId);
                var accessTokens = await _accessTokenService.GetBudgetAccessTokensAsync(budget.Id);
                foreach (var accessToken in accessTokens)
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
                            expiredAccessItems.Add(new ExpiredAccessItem(accessToken.AccessToken, fex.Error.error_message, institution));
                        }
                    }
                }
                var transactionCategories = await _categoryService.GetTransactionCategoriesAsync(request.BudgetId);
                var transactionCategoryData = from t in allTransactions
                                              join c in transactionCategories on t.transaction_id equals c.TransactionId into tmpC
                                              from c in tmpC.DefaultIfEmpty()
                                              select new
                                              {
                                                  Transaction = t,
                                                  Category = c == null
                                                  ? null
                                                  : new CategoryModel
                                                  {
                                                      Id = c.CategoryId,
                                                      Name = c.CategoryName
                                                  }
                                              };
                return new Response
                {
                    Transactions = transactionCategoryData.Select(d =>
                    {
                        var transaction = d.Transaction;
                        transaction.Category = d.Category;
                        return transaction;
                    }).ToList(),
                    ExpiredAccessItems = expiredAccessItems
                };
            }
        }
        public class Response
        {
            public List<TransactionModel> Transactions { get; set; }
            public List<ExpiredAccessItem> ExpiredAccessItems { get; set; }
        }
    }
}
