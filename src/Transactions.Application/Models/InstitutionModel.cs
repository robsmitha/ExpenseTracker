using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transactions.Application.Mappings;
using Transactions.Domain.Responses;

namespace Transactions.Application.Models
{
    public class InstitutionModel : IMapFrom<InstitutionResponse.Institution>
    {
        public string institution_id { get; set; }
        public string name { get; set; }
    }
}
