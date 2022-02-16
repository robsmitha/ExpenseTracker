using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transactions.Application.Models
{
    public class UserAccessItemModel
    {
        public ItemModel Item { get; set; }
        public InstitutionModel Institution { get; set; }
        public List<AccountModel> Accounts { get; set; }
    }
}
