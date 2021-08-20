using AutoMapper;
using Module.Order.Business.Models;

namespace Module.Cart.WebUI.Models.Mappings {
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<CartItemViewModel, CartItem1CDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => 0));
        }
    }
}