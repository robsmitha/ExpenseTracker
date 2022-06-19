
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transactions.Application.Models
{
    public record ExpiredAccessItem(string AccessToken, string Message, InstitutionModel Institution);
    public record AccessItem(ItemModel Item, InstitutionModel Institution, List<AccountModel> Accounts);
    public class UserInstitutionsModel
    {
        public List<UserAccessItemModel> AccessItems { get; set; }
        public List<ExpiredAccessItem> ExpiredAccessItems { get; set; }
    }
}
