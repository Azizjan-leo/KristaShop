using AutoMapper;
using KristaShop.Common.Models.Filters;
using Module.Catalogs.Business.Models;

namespace Module.Catalogs.WebUI.Models.Mappings {
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<ModelDescriptor1CDTO, ModelDescriptorViewModel>();
            
            CreateMap<NomFilterDTO, ProductsFilterExtended>()
                .ForMember(dest => dest.CatalogId, opt => opt.MapFrom(src => src.CatalogId))
                .ForMember(dest => dest.MinPrice, opt => opt.MapFrom(src => src.MinPrice ?? -1D))
                .ForMember(dest => dest.MaxPrice, opt => opt.MapFrom(src => src.MaxPrice ?? -1D))
                .ForMember(dest => dest.ColorIds, opt => opt.MapFrom(src => src.Colors))
                .ForMember(dest => dest.CategoriesIds, opt => opt.MapFrom(src => src.Categories))
                .ForMember(dest => dest.IncludeCategoriesMap, opt => opt.MapFrom(src => false));
        }
    }
}