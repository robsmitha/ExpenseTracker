using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions.Application.Mappings;
using Transactions.Domain.Entities;

namespace Transactions.Application.Models
{
    public class AccessTokenModel : IMapFrom<AccessToken>
    {
        public string UserId { get; set; }
        public string Token { get; set; }
    }
}
