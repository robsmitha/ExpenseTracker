using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions.Application.Mappings;
using Transactions.Domain.Entities;

namespace Transactions.Application.Models
{
    public class TransactionCategoryModel : IMapFrom<TransactionCategory>
    {
        public int Id { get; set; }
        public string TransactionId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int BudgetId { get; set; }
    }
}
