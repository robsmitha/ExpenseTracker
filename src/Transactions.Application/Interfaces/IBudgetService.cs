using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions.Application.Models;

namespace Transactions.Application.Interfaces
{
    public interface IBudgetService
    {
        Task<List<BudgetModel>> GetBudgetsAsync();
        Task<BudgetModel> GetBudgetAsync(int budgetId);
        Task<BudgetModel> AddBudgetAsync(BudgetModel model);
        Task<BudgetModel> UpdateBudgetAsync(BudgetModel model);
        Task<List<BudgetCategoryModel>> GetBudgetCategoriesAsync(int budgetId);
        Task<BudgetCategoryModel> UpdateBudgetCategoryEstimateAsync(int budgetId, int categoryId, decimal estimate);
        Task AddBudgetCategoryAsync(int budgetId, int categoryId, decimal? estimate = null);
        Task<BudgetExcludedTransactionModel> SetExcludedTransactionAsync(int budgetId, string transactionId);
        Task<bool> RestoreExcludedTransactionAsync(int budgetId, string transactionId);
        Task<List<BudgetExcludedTransactionModel>> GetExcludedTransactionsAsync(int budgetId);
    }
}
