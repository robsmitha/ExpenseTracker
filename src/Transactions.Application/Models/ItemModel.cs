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
    public class ItemModel : IMapFrom<ItemResponse>
    {
        public string ItemId { get; set; }
        public string InstitutionId { get; set; }
        public DateTime? LastSuccessfulUpdate { get; set; }
        public string ErrorDisplayMessage { get; set; }
        public string ErrorCode { get; set; }
        public bool HasError => !string.IsNullOrWhiteSpace(ErrorCode);
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ItemResponse, ItemModel>()
                       .ForMember(dest => dest.ItemId, act => act.MapFrom(a => a.item.item_id))
                       .ForMember(dest => dest.InstitutionId, act => act.MapFrom(a => a.item.institution_id))
                       .ForMember(dest => dest.ErrorDisplayMessage, act => act.MapFrom(a => a.item.error != null ? a.item.error.display_message : string.Empty))
                       .ForMember(dest => dest.ErrorCode, act => act.MapFrom(a => a.item.error != null ? a.item.error.error_code : string.Empty))
                       .ForMember(dest => dest.LastSuccessfulUpdate, act => act.MapFrom(a => a.status.transactions.last_successful_update));
        }
    }
}
