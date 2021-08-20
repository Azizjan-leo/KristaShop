using AutoMapper;
using Module.Order.Business.Models;

namespace Module.Order.Admin.Admin.Models.Mappings {
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<OrdersTotalFilter, OrdersTotalFilterDTO>();
        }
    }
}