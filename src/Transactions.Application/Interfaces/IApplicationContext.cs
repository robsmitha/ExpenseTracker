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
        DbSet<Category> Categories { get; set; }
        DbSet<TransactionCategory> TransactionCategories { get; set; }
    }
}
