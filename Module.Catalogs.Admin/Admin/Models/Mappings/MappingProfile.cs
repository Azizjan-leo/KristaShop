using System.Linq;
using AutoMapper;
using KristaShop.Common.Enums;
using KristaShop.Common.Extensions;
using KristaShop.Common.Helpers;
using Module.Catalogs.Business.Models;

namespace Module.Catalogs.Admin.Admin.Models.Mappings {
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<CategoryViewModel, CategoryDTO>();
            CreateMap<CategoryDTO, CategoryViewModel>();

            CreateMap<CatalogViewModel, CatalogDTO>()
                .ForMember(x => x.Preview, opt => opt.Ignore());
            CreateMap<CatalogDTO, CatalogViewModel>();

            CreateMap<CatalogItemGroupNew, AdminCatalogItemBriefViewModel>()
                .ForMember(dest => dest.Articul, opt => opt.MapFrom(src => src.Descriptor.Articul))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CatalogItems.First().Items.First().ModelName))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.CatalogItems.Sum(x => x.Items.Sum(c => c.Amount))))
                .ForMember(dest => dest.MainPhoto, opt => opt.MapFrom(src => src.Descriptor.MainPhoto))
                .ForMember(dest => dest.IsVisible, opt => opt.MapFrom(src => src.Descriptor.IsVisible))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => $"{src.CommonPrice.ToTwoDecimalPlaces()}$"))
                .ForMember(dest => dest.PriceInRub, opt => opt.MapFrom(src => $"{src.CommonPriceInRub.ToTwoDecimalPlaces()}р"))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.Descriptor.AddDate))
                .ForMember(dest => dest.Colors, opt => opt.MapFrom(src => src.Colors.Select(i => i.Name).ToArray()))
                .ForMember(dest => dest.Sizes, opt => opt.MapFrom(src => src.Sizes.ToArray()))
                .ForMember(dest => dest.Catalogs, opt => opt.MapFrom(src => src.InCatalogs.Select(i => i.Name).ToArray()))
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.InCategories.Select(i => i.Name).ToArray()));

            CreateMap<CatalogItemGroupNew, CatalogItem1CBriefViewModel>()
                .ForMember(dest => dest.Articul, opt => opt.MapFrom(src => src.Descriptor.Articul))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CatalogItems.First().Items.First().ModelName))
                .ForMember(dest => dest.MainPhoto, opt => opt.MapFrom(src => src.Descriptor.MainPhoto))
                .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Order))
                .ForMember(dest => dest.IsVisible, opt => opt.MapFrom(src => src.Descriptor.IsVisible))
                .ForMember(dest => dest.CatalogId, opt => opt.MapFrom(src => src.CatalogId))
                .ForMember(dest => dest.AllCatalogs, opt => opt.MapFrom(src => src.InCatalogs))
                .ForMember(dest => dest.CatalogItems,
                    opt => opt.MapFrom(src => src.CatalogItems.SelectMany(x => x.Items).OrderBy(x => x.Color.Name).ThenBy(x => x.Size.Value, new SizeStringComparer())));
            
            CreateMap<CatalogItemGroupNew, Model1CViewModel>()
                .ForMember(dest => dest.Articul, opt => opt.MapFrom(src => src.Descriptor.Articul))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Descriptor.Description))
                .ForMember(dest => dest.Matherial, opt => opt.MapFrom(src => src.Descriptor.Material))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.Descriptor.AddDate))
                .ForMember(dest => dest.DefaultPrice, opt => opt.MapFrom(src => src.CommonPrice))
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.Descriptor.MainPhoto))
                .ForMember(dest => dest.IsLimited, opt => opt.MapFrom(src => src.Descriptor.IsLimited))
                .ForMember(dest => dest.IsVisible, opt => opt.MapFrom(src => src.Descriptor.IsVisible))
                .ForMember(dest => dest.Photos, opt => opt.Ignore())
                .ForMember(dest => dest.Catalogs, opt => opt.MapFrom(src => src.InCatalogs.Select(x => x.Name).ToList()))
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.InCategories.Select(x => x.Name).ToList()))
                .ForMember(dest => dest.Matherial, opt => opt.MapFrom(src => src.Descriptor.Material))
                .ForMember(dest => dest.MetaTitle, opt => opt.MapFrom(src => src.Descriptor.MetaTitle))
                .ForMember(dest => dest.MetaKeywords, opt => opt.MapFrom(src => src.Descriptor.MetaKeywords))
                .ForMember(dest => dest.MetaDescription, opt => opt.MapFrom(src => src.Descriptor.MetaDescription))
                .ForMember(dest => dest.VideoUrl, opt => opt.MapFrom(src => src.Descriptor.VideoLink));

            CreateMap<Model1CViewModel, UpdateCatalogItemDescriptorDTO>()
                .ForMember(dest => dest.VideoLink, opt => opt.MapFrom(src => src.VideoUrl))
                .ForMember(dest => dest.AddDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.MainPhoto, opt => opt.MapFrom(src => src.ImagePath))
                .ForMember(dest => dest.AltText, opt => opt.MapFrom(src => src.ImageAlternativeText))
                .ForMember(dest => dest.Material, opt => opt.MapFrom(src => src.Matherial))
                .ForMember(dest => dest.CatalogsInvisibility,
                    opt => opt.MapFrom(src => src.CatalogsInvisibility.Select(x => x.ToProductCatalog1CId())));
            
            CreateMap<CatalogDTO, CatalogViewModel>();
            CreateMap<CatalogExtraChargeDTO, CatalogExtraChargeViewModel>();
        }
    }
}