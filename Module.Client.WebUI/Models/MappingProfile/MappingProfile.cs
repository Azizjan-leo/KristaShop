using AutoMapper;
using Module.Client.Business.Models;

namespace Module.Client.WebUI.Models.MappingProfile {
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<RegisterViewModel, NewUserDTO>();
        }
    }
}