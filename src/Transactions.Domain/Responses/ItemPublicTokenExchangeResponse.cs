using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transactions.Domain.Responses
{
    public class ItemPublicTokenExchangeResponse
    {
        public string access_token { get; set; }
        public string item_id { get; set; }
        public string request_id { get; set; }
    }
}
