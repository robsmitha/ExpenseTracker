using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions.Application.Mappings;
using Transactions.Domain.Entities;

namespace Transactions.Application.Models
{
    public class BudgetAccessItemModel
    {
        public int UserAccessItemId { get; set; }
        public string InstitutionName { get; set; }
    }
    public class BudgetModel : IMapFrom<Budget>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<CategoryModel> Categories { get; set; }
        public List<BudgetAccessItemModel> BudgetAccessItems { get; set; }

        public bool IsExisting => Id > 0;
    }
}
