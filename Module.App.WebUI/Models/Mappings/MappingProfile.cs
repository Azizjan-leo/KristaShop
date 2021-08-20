using AutoMapper;
using KristaShop.Common.Models.Session;
using Module.App.Business.Models;

namespace Module.App.WebUI.Models.Mappings {
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<UserSessionInfo, FeedbackViewModel>()
                .ForMember(dest => dest.Person, opt => opt.MapFrom(source => source.FullName))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(source => source.Phone))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(source => source.Email));

            CreateMap<UserSessionInfo, FeedbackDTO>()
                .ForMember(dest => dest.Person, opt => opt.MapFrom(source => source.FullName))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(source => source.Phone))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(source => source.Email));

            CreateMap<FeedbackViewModel, FeedbackDTO>();
            CreateMap<AuthFeedbackViewModel, FeedbackDTO>();
            CreateMap<ManagementContactsFeedbackViewModel, FeedbackDTO>();
        }
    }
}