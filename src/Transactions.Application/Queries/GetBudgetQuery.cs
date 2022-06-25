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
                var allTransactions = new List<TransactionModel>();
                var expiredAccessTokens = new List<ExpiredAccessItem>();
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
                            expiredAccessTokens.Add(new ExpiredAccessItem(accessToken.AccessToken, fex.Error.error_message, institution));
                        }
                    }
                }

                var budgetCategories = await _budgetService.GetBudgetCategoriesAsync(request.BudgetId);
                var transactionCategories = await _categoryService.GetTransactionCategoriesAsync(request.BudgetId);
                var transactionCategoryData = from t in allTransactions
                        join c in transactionCategories on t.transaction_id equals c.TransactionId into tmpC
                        from c in tmpC.DefaultIfEmpty()
                        group t.amount by c?.CategoryName ?? "Uncategorized"
                        into g
                        select new
                        {
                            Category = g.Key,
                            Sum = (decimal)g.Sum()
                        };

                var budgetCategoryData = new List<TransactionCategoryData>();
                
                var uncategorizedCategory = transactionCategoryData.FirstOrDefault(g => g.Category == "Uncategorized");
                if(uncategorizedCategory != null)
                {
                    budgetCategoryData.Add(new TransactionCategoryData
                    {
                        Category = uncategorizedCategory.Category,
                        Sum = uncategorizedCategory.Sum,
                        Estimate = uncategorizedCategory.Sum
                    });
                }

                foreach (var budgetCategory in budgetCategories)
                {
                    var transactionData = transactionCategoryData.FirstOrDefault(c => c.Category == budgetCategory.CategoryName);
                    budgetCategoryData.Add(new TransactionCategoryData
                    {
                        Estimate = budgetCategory.Estimate,
                        Category = budgetCategory.CategoryName,
                        Sum = transactionData?.Sum ?? 0
                    });
                }

                var transactionsTotal = (decimal)allTransactions.Sum(t => t.amount);
                return new BudgetViewModel(budgetCategoryData, budget.Name, budget.StartDate, budget.EndDate, expiredAccessTokens, transactionsTotal);
            }
        }

    }
}
