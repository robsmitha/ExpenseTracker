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
    public class TransactionModel : IMapFrom<TransactionResponse.Transaction>
    {
        public string date { get; set; }
        public string name { get; set; }
        public double amount { get; set; }
        public string transaction_id { get; set; }
        public bool pending { get; set; }
        public string authorized_date { get; set; }
        public string transaction_type { get; set; }
        public string payment_channel { get; set; }
        public string merchant_name { get; set; }
        public string account_id { get; set; }

        public bool HasTransactionCategory => Category != null;
        public CategoryModel Category { get; set; }
        public AccountModel Account { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<TransactionResponse.Transaction, TransactionModel>()
                    .ForMember(dest => dest.Account, act => act.Ignore())
                    .ForMember(dest => dest.Category, act => act.Ignore());
        }
    }
}
