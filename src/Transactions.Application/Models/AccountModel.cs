using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions.Application.Mappings;
using Transactions.Domain.Responses;

namespace Transactions.Application.Models
{
    public class AccountModel : IMapFrom<TransactionResponse.Account>
    {
        public string account_id { get; set; }
        public string mask { get; set; }
        public string name { get; set; }
        public string official_name { get; set; }
        public string subtype { get; set; }
        public string type { get; set; }
    }
}
