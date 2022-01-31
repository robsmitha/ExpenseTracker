using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions.Application.Mappings;
using Transactions.Domain.Responses;

namespace Transactions.Application.Models
{
    public class ItemPublicTokenExchangeModel : IMapFrom<ItemPublicTokenExchangeResponse>
    {
        public string access_token { get; set; }
        public string item_id { get; set; }
        public string request_id { get; set; }
    }
}
