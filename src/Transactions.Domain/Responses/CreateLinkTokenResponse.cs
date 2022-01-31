using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transactions.Domain.Responses
{
    public class CreateLinkTokenResponse
    {
        public DateTime expiration { get; set; }
        public string link_token { get; set; }
        public string request_id { get; set; }
    }
}
