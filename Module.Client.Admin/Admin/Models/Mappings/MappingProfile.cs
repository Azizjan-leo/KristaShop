using AutoMapper;
using Module.Client.Business.Models;

namespace Module.Client.Admin.Admin.Models.Mappings {
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<NewUserViewModel, NewUserDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.CityId, opt => opt.MapFrom(src => src.CityId))
                .ForMember(dest => dest.NewCity, opt => opt.MapFrom(src => src.OtherCity))
                .ForMember(dest => dest.MallAddress, opt => opt.MapFrom(src => src.MallAddress))
                .ForMember(dest => dest.CompanyAddress, opt => opt.MapFrom(src => src.CompanyAddress))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.Login))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.ManagerId, opt => opt.MapFrom(src => src.ManagerId))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId));
            
            CreateMap<UserClientDTO, UserUpdateViewModel>();

            CreateMap<UserUpdateViewModel, NewUserDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.NewUserId))
                .ForMember(dest => dest.NewCity, opt => opt.MapFrom(src => string.Empty));
        }
    }
}