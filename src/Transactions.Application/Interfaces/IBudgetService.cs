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
    }
}
