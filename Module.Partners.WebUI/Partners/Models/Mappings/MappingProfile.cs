using AutoMapper;
using KristaShop.Common.Models.Structs;
using Module.Common.Business.Models;

namespace Module.Partners.WebUI.Partners.Models.Mappings {
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<ModelAmountVM, ModelAmount>()
                .ForMember(dest => dest.Size, opt => opt.MapFrom(src => new SizeValue(src.Size)));
        }
    }
}