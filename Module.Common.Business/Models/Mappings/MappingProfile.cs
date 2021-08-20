using System.Linq;
using AutoMapper;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.DataFrom1C;

namespace Module.Common.Business.Models.Mappings {
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<Settings, SettingsDTO>();
            CreateMap<SettingsDTO, Settings>();
            
            CreateMap<BarcodeShipmentsSqlView, BarcodeShipmentItemDTO>();
            CreateMap<Collection, CollectionDTO>();
        
            CreateMap<IGrouping<string, BarcodeShipmentsSqlView>, ModelGroupedDTO>()
                .ForMember(dest => dest.ModelKey, opt => opt.MapFrom(src => src.Key))
                .ForMember(dest => dest.ModelInfo, opt => opt.MapFrom(src => src.First()))
                .ForMember(dest => dest.Colors, opt => opt.MapFrom(src => src.GroupBy(c => c.ColorId).Select(c => new ColorDTO() {Id = c.First().ColorId, Name = c.First().ColorName, Code = c.First().ColorValue, Image = c.First().ColorPhoto})))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.Sum(x=>x.Amount)))
                .ForMember(dest => dest.TotalAmountByColor, opt => opt.MapFrom(src => src.GroupBy(c => c.ColorId).ToDictionary(k => k.Key, v => v.Sum(c => c.Amount))))
                .ForMember(dest => dest.TotalSum, opt => opt.MapFrom(src => src.Sum(c => c.Amount * c.Price)))
                .ForMember(dest => dest.TotalSumByColor, opt => opt.MapFrom(src => src.GroupBy(c => c.ColorId).ToDictionary(k => k.Key, v => v.Sum(c => c.Amount * c.Price))))
                .ForMember(dest => dest.SizesInfo, opt => opt.MapFrom(src => new SizesAmountsDTO {
                Values = src.SelectMany(c => c.Size.Values).Distinct().OrderBy(c => c).ToList(),
                SizeColorAmount = src.SelectMany(x=>x.Size.Values, (p, c) => new { Size = c, ColorId = p.ColorId, Amount = p.Amount / p.Size.Parts }).GroupBy(c => $"{c.Size}_{c.ColorId}").ToDictionary(k => k.Key, v => v.Sum(c => c.Amount)),
                TotalAmountBySize = src.SelectMany(x=>x.Size.Values, (p, c) => new { Size = c, Amount = p.Amount / p.Size.Parts }).GroupBy(c => c.Size).ToDictionary(k => k.Key, v => v.Sum(c => c.Amount)),
                }))
                .ForMember(dest => dest.Barcodes, opt => opt.MapFrom(src => src.SelectMany(x => x.Barcodes, (x, b) => new BarcodeDTO { Barcode = b, ColorId = x.ColorId, Size = x.Size})));
                
            CreateMap<BarcodeShipmentsSqlView, ModelInfoDTO>();

            CreateMap<Color, Color1CDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.GroupId, opt => opt.MapFrom(src => src.GroupId));
            
            CreateMap<CatalogItemFull, Color1CDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ColorId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ColorName))
                .ForMember(dest => dest.GroupId, opt => opt.MapFrom(src => src.ColorGroupId))
                .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.ColorGroupName))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => (string.IsNullOrEmpty(src.ColorValue) ? "#F5F5F5" : src.ColorValue)))
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.ColorPhoto));
            
            CreateMap<City, CityDTO>();
            CreateMap<CityDTO, City>();
        }
    }
}