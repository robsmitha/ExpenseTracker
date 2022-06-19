using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions.Domain.Entities;

namespace Transactions.Application.Interfaces
{
    public interface IApplicationContext
    {
        DbSet<Budget> Budgets { get; set; }
        DbSet<BudgetAccessItem> BudgetAccessItems { get; set; }
        DbSet<BudgetCategory> BudgetCategories { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<TransactionCategory> TransactionCategories { get; set; }
        DbSet<UserAccessItem> UserAccessItems { get; set; }
    }
}
