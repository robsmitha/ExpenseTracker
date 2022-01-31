using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions.Application.Mappings;
using Transactions.Domain.Responses;

namespace Transactions.Application.Models
{
    public class LinkTokenModel : IMapFrom<CreateLinkTokenResponse>
    {
        public DateTime expiration { get; set; }
        public string link_token { get; set; }
        public string request_id { get; set; }
    }
}
