using System;
using AutoMapper;
using KristaShop.Common.Extensions;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.DataFor1C;
using KristaShop.DataAccess.Entities.DataFrom1C;

namespace Module.Client.Business.Models.Mappings {
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<NewUserDTO, NewUser>();
            CreateMap<NewUser, NewUserDTO>();
            CreateMap<NewUserSqlView, NewUserDTO>();

            CreateMap<NewUser, UserClientDTO>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId ?? 0))
                .ForMember(dest => dest.NewUserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.NewCity));

            CreateMap<NewUserSqlView, UserClientDTO>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId ?? 0))
                .ForMember(dest => dest.NewUserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.NewCity) ? src.CityName : src.NewCity));
            
            CreateMap<UserSqlView, UserClientDTO>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ManagerName, opt => opt.MapFrom(src => (string.IsNullOrEmpty(src.ManagerName) ? "---" : src.ManagerName)))
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.CreateDate.ToUserCreateDate()));

            CreateMap<User, UserClientDTO>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ManagerName, opt => opt.MapFrom(src => src.Manager != null ? src.Manager.Name : "---"))
                .ForMember(dest => dest.LastSignIn, opt => opt.MapFrom(src => src.Data != null ? src.Data.LastSignIn : DateTimeOffset.MinValue))
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City != null ? src.City.Name : ""))
                .ForMember(dest => dest.Catalogs, opt => opt.MapFrom(src => src.GetAccessesToCatalogs()));
            
            CreateMap<AuthorizationLink, AuthorizationLinkDTO>();
        }
    }
}