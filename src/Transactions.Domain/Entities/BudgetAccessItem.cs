using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transactions.Domain.Entities
{
    public class BudgetAccessItem
    {
        public int Id { get; set; }
        public int BudgetId { get; set; }
        [ForeignKey(nameof(BudgetId))]
        public Budget Budget { get; set; }
        public int UserAccessItemId { get; set; }
        [ForeignKey(nameof(UserAccessItemId))]
        public UserAccessItem UserAccessItem { get; set; }
    }
}
