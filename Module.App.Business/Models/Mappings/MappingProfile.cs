using System;
using System.Linq;
using AutoMapper;
using KristaShop.Common.Extensions;
using KristaShop.Common.Models.DTOs;
using KristaShop.Common.Models.Session;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.DataFor1C;
using KristaShop.DataAccess.Entities.DataFrom1C;
using KristaShop.DataAccess.Views;

namespace Module.App.Business.Models.Mappings {
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<MenuItem, MenuItemDTO>()
                .ForMember(dest => dest.ChildControllers,
                    dest => dest.MapFrom(item => item.ChildItems.Select(x => x.ControllerName)));
            CreateMap<MenuItemDTO, MenuItem>();

            CreateMap<AppliedImportItem, AppliedImportDTO>();
            CreateMap<AppliedImportDTO, AppliedImportItem>();

            CreateMap<PromoLink, PromoLinkDTO>();
            CreateMap<PromoLinkDTO, PromoLink>();

            CreateMap<Feedback, FeedbackDTO>()
                .ForMember(dest => dest.FormattedDate, dest => dest.MapFrom(item => item.RecordTimeStamp.ToBasicString()));

            CreateMap<FeedbackDTO, Feedback>()
                .ForMember(dest => dest.Id, dest => dest.MapFrom(item => Guid.NewGuid()))
                .ForMember(dest => dest.Viewed, dest => dest.MapFrom(item => false))
                .ForMember(dest => dest.RecordTimeStamp, dest => dest.MapFrom(item => DateTime.Now));
            CreateMap<FeedbackCreateFileDTO, FeedbackFile>()
                .ForMember(dest => dest.Id, dest => dest.MapFrom(item => Guid.NewGuid()))
                .ForMember(dest => dest.Filename, dest => dest.MapFrom(item => item.OriginalName));

            CreateMap<FeedbackFile, FeedbackFileDTO>();
            CreateMap<FeedbackFileDTO, FeedbackFile>()
                .ForMember(dest => dest.Id, dest => dest.MapFrom(item => Guid.NewGuid()));

            CreateMap<Role, RoleDTO>();
            CreateMap<RoleDTO, Role>();

            CreateMap<RoleAccess, RoleAccessDTO>();
            CreateMap<RoleAccessDTO, RoleAccess>();
            
            CreateMap<UserSqlView, UserSessionInfo>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.MallAddress, opt => opt.MapFrom(src => src.Address));

            CreateMap<UserSqlView, UserSession>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.Login))
                .ForMember(dest => dest.IsRoot, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.IsManager, opt => opt.MapFrom(src => src.IsManager))
                .ForMember(dest => dest.ManagerId, opt => opt.MapFrom(src => src.ManagerId))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src));

            CreateMap<User, UserSession>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.IsRoot, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src));

            CreateMap<User, UserSessionInfo>();
            
            //TODO: move this mappings out of module
            CreateMap<Manager, ManagerDTO>();
            CreateMap<ManagerDetailsSqlView, ManagerDetailsDTO>();
            CreateMap<ManagerDetails, ManagerDetailsDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ManagerId));
            CreateMap<ManagerAccess, ManagerAccessDTO>();
            CreateMap<ManagerAccessDTO, ManagerAccess>();
        }
    }
}