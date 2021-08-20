using AutoMapper;
using KristaShop.DataAccess.Entities.Media;

namespace Module.Media.Business.DTOs.Mappings {
    public class DefaultMappingProfile : Profile {
        public DefaultMappingProfile() {
            CreateMap<DynamicPage, DynamicPageDTO>();
            CreateMap<DynamicPageDTO, DynamicPage>()
                .ForMember(dest => dest.Url, dest => dest.MapFrom(item => item.URL));
            
            CreateMap<BlogItem, BlogItemDTO>();
            CreateMap<BlogItemDTO, BlogItem>();

            CreateMap<BannerItem, BannerItemDTO>();
            CreateMap<BannerItemDTO, BannerItem>();

            CreateMap<GalleryItem, GalleryItemDTO>();
            CreateMap<GalleryItemDTO, GalleryItem>();

            CreateMap<Faq, FaqDTO>();
            CreateMap<FaqSection, FaqSectionDto>();

            CreateMap<FaqSectionContent, FaqSectionContentDto>()
                .ForMember(dest => dest.FaqSectionContentFileDtos,
                    dest => dest.MapFrom(item => item.FaqSectionContentFiles));

            CreateMap<FaqSectionContentFile, FaqSectionContentFileDto>();
        }
    }
}