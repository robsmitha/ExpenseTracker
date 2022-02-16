using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions.Application.Mappings;
using Transactions.Domain.Entities;

namespace Transactions.Application.Models
{
    public class AccessTokenModel : IMapFrom<UserAccessItem>
    {
        public string UserId { get; set; }
        public string AccessToken { get; set; }
        public string ItemId { get; set; }
    }
}
