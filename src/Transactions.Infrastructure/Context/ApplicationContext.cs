using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions.Application.Interfaces;
using Transactions.Domain.Entities;

namespace Transactions.Infrastructure.Context
{
    public class ApplicationContext : DbContext, IApplicationContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
               : base(options)
        { }

        public DbSet<Budget> Budgets { get; set; }
        public DbSet<BudgetAccessItem> BudgetAccessItems { get; set; }
        public DbSet<BudgetCategory> BudgetCategories { get; set; }
        public DbSet<BudgetExcludedTransaction> BudgetExcludedTransactions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<TransactionCategory> TransactionCategories { get; set; }
        public DbSet<UserAccessItem> UserAccessItems { get; set; }
    }
}
