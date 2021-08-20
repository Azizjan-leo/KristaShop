using AutoMapper;
using KristaShop.Common.Models.DTOs;
using Module.App.Business.Models;
using Module.Common.Admin.Admin.Models;
using Module.Common.Business.Models;

namespace Module.App.Admin.Admin.Models.Mappings {
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<SettingEditViewModel, SettingsDTO>();
            CreateMap<SettingsDTO, SettingEditViewModel>();
            
            CreateMap<MenuItemViewModel, MenuItemDTO>();
            CreateMap<MenuItemDTO, MenuItemViewModel>();
            
            CreateMap<ManagerDetailsViewModel, ManagerDetailsDTO>();
            CreateMap<ManagerDetailsDTO, ManagerDetailsViewModel>();

            CreateMap<RoleDTO, RoleViewModel>();
            CreateMap<RoleViewModel, RoleDTO>();
            CreateMap<RoleAccessDTO, RoleAccessViewModel>();
            CreateMap<RoleAccessViewModel, RoleAccessDTO>();
            
            CreateMap<PromoLinkDTO, PromoLinkViewModel>();
            CreateMap<PromoLinkViewModel, PromoLinkDTO>()
                .ForMember(x => x.VideoPreview, opt => opt.Ignore());
            
            CreateMap<FeedbackFileDTO, FileViewModel>()
                .ForMember(dest => dest.Id, dest => dest.MapFrom(item => item.Id))
                .ForMember(dest => dest.Filename, dest => dest.MapFrom(item => item.Filename));
        }
    }
}