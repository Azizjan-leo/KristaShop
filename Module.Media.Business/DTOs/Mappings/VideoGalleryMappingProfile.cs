using System.Linq;
using AutoMapper;
using KristaShop.DataAccess.Entities.Media;

namespace Module.Media.Business.DTOs.Mappings {
    public class VideoGalleryMappingProfile : Profile {
        public VideoGalleryMappingProfile() {
            #region map videos

            CreateMap<Video, VideoDTO>()
                .ForMember(dest => dest.GalleryIds, opt => opt.MapFrom(src => src.VideoGalleryVideos.Select(x => x.GalleryId)));

            CreateMap<VideoDTO, Video>();

            #endregion

            #region map video gallery

            CreateMap<VideoGallery, VideoGalleryDTO>()
                .ForMember(dest => dest.VideosCount, dest => dest.MapFrom(src => src.VideoGalleryVideos.Count));

            CreateMap<VideoGalleryDTO, VideoGallery>();

            var quantity = 0;
            CreateMap<VideoGallery, VideoGalleryWithVideosDTO>()
                .ForMember(dest => dest.Videos, opt => opt.MapFrom(src => src.VideoGalleryVideos.Where(x => x.Video.IsVisible).OrderBy(x => x.Order).Select(x => x.Video).Take(quantity)));
            
            #endregion

            #region map video gallery videos

            CreateMap<VideoGalleryVideos, VideoDTO>()
                .ForMember(dest => dest.Id, dest => dest.MapFrom(src => src.Video.Id))
                .ForMember(dest => dest.PreviewPath, dest => dest.MapFrom(src => src.Video.PreviewPath))
                .ForMember(dest => dest.VideoPath, dest => dest.MapFrom(src => src.Video.VideoPath))
                .ForMember(dest => dest.Title, dest => dest.MapFrom(src => src.Video.Title))
                .ForMember(dest => dest.Description, dest => dest.MapFrom(src => src.Video.Description))
                .ForMember(dest => dest.IsVisible, dest => dest.MapFrom(src => src.Video.IsVisible))
                .ForMember(dest => dest.Order, dest => dest.MapFrom(src => src.Order));
            #endregion
        }
    }
}
