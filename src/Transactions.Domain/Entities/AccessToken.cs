using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transactions.Domain.Entities
{
    public class AccessToken
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }
    }
}
