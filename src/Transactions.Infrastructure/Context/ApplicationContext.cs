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

        public DbSet<AccessToken> AccessTokens { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<TransactionCategory> TransactionCategories { get; set; }
    }
}
