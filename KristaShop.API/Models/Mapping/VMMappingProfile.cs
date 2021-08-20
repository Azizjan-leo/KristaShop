using AutoMapper;
using KristaShop.API.Models.Responses;
using Module.Common.Business.Models;
using Module.Partners.Business.DTOs;

namespace KristaShop.API.Models.Mapping {
    public class VMMappingProfile : Profile{
        public VMMappingProfile() {
            CreateMap<PartnerStorehouseItemDTO, PartnerStorehouseItemVM>()
                .ForMember(dest => dest.SizeValue, dest => dest.MapFrom(item => item.Size.Value))
                .ForMember(dest => dest.PartsCount, dest => dest.MapFrom(item => item.Size.Parts));
            
            CreateMap<BarcodeShipmentItemDTO, PartnerStorehouseItemVM>()
                .ForMember(dest => dest.SizeValue, dest => dest.MapFrom(item => item.Size.Value))
                .ForMember(dest => dest.Date, dest => dest.MapFrom(item => item.SaleDate))
                .ForMember(dest => dest.PartsCount, dest => dest.MapFrom(item => item.Size.Parts));

            CreateMap<BarcodeShipmentItemDTO, BarcodeShipmentItemVM>()
                .ForMember(dest => dest.SizeValue, dest => dest.MapFrom(item => item.Size.Value))
                .ForMember(dest => dest.Date, dest => dest.MapFrom(item => item.SaleDate));
        }
    }
}
