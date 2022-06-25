using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions.Application.Mappings;

namespace Transactions.Application.Models
{
    public class SaveCategoryModel : CategoryModel
    {
        public int BudgetId { get; set; }
        public decimal? Estimate { get; set; }
    }
}
