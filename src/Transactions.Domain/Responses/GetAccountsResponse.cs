using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transactions.Domain.Responses
{
    public class GetAccountsResponse
    {
        public List<Account> accounts { get; set; }
        public Item item { get; set; }
        public string request_id { get; set; }
    }
}
