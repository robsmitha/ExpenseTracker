
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transactions.Application.Models
{
    public record AccessItem(ItemModel Item, InstitutionModel Institution, List<AccountModel> Accounts);
    public class UserInstitutionsModel
    {
        public List<UserAccessItemModel> AccessItems { get; set; }
    }
}
