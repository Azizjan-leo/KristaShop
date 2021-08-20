using AutoMapper;
using Module.Media.Business.DTOs;

namespace Module.Media.Admin.Admin.Models.Mappings {
    public class DefaultMappingProfile : Profile {
        public DefaultMappingProfile() {
            CreateMap<BlogItemViewModel, BlogItemDTO>();
            CreateMap<BlogItemDTO, BlogItemViewModel>();

            CreateMap<GalleryItemViewModel, GalleryItemDTO>();
            CreateMap<GalleryItemDTO, GalleryItemViewModel>();

            CreateMap<BannerItemViewModel, BannerItemDTO>()
                .ForMember(x => x.Image, opt => opt.Ignore());
            CreateMap<BannerItemDTO, BannerItemViewModel>()
                .ForMember(x => x.Image, opt => opt.Ignore());
            
            CreateMap<VideoDTO, VideoViewModel>();
            CreateMap<VideoViewModel, VideoDTO>()
                .ForMember(x => x.Preview, opt => opt.Ignore());

            CreateMap<VideoGalleryDTO, VideoGalleryViewModel>();
            CreateMap<VideoGalleryViewModel, VideoGalleryDTO>()
                .ForMember(x => x.Preview, opt => opt.Ignore());
            
            CreateMap<DynamicPageDTO, DynamicPageViewModel>()
                .ForMember(x => x.LayoutName, opt => opt.MapFrom(src => DynamicPageLayout.TryGetLayoutTitle(src.Layout)));

            CreateMap<DynamicPageViewModel, DynamicPageDTO>()
                .ForMember(x => x.TitleIcon, opt => opt.Ignore())
                .ForMember(x => x.Image, opt => opt.Ignore());
        }
    }
}