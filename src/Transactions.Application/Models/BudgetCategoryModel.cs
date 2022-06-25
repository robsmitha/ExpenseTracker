using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions.Application.Mappings;
using Transactions.Domain.Entities;

namespace Transactions.Application.Models
{
    public class BudgetCategoryModel : IMapFrom<BudgetCategory>
    {
        public int Id { get; set; }
        public decimal Estimate { get; set; }
        public int BudgetId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
