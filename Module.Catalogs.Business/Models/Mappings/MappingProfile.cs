using AutoMapper;
using KristaShop.Common.Enums;
using KristaShop.Common.Extensions;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.DataFrom1C;
using Module.Common.Business.Models;

namespace Module.Catalogs.Business.Models.Mappings {
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<CatalogItemBrief, CatalogItemBriefDTO>();
            CreateMap<CatalogItemBriefDTO, CatalogItemBrief>();
            
            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryDTO, Category>();

            CreateMap<Catalog, CatalogDTO>()
                .ForMember(dest => dest.OrderFormName, dest => dest.MapFrom(item => (item.OrderForm).GetDisplayName()))
                .ForMember(dest => dest.Id, dest => dest.MapFrom(item => (int) item.Id))
                .ForMember(dest => dest.Catalog1CName, dest => dest.MapFrom(item => item.Id.GetDisplayName()));

            CreateMap<CatalogDTO, Catalog>();
            
            CreateMap<Category1C, Category1CDTO>();

            CreateMap<ModelToCatalog1CMap, Catalog1CDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CatalogId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CatalogId.ToProductCatalog1CId().AsString()));

            CreateMap<ModelToCategory1CMap, Category1CDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CategoryName));

            CreateMap<CatalogItemDescriptor, CatalogItemDescriptorDTO>()
                .ForMember(dest => dest.Material, opt => opt.MapFrom(src => src.Matherial));
            
            CreateMap<CatalogItemFull, ColorDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ColorId))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.ColorValue))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.ColorPhoto))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ColorName));

            CreateMap<CatalogItemFull, ModelDescriptor1CDTO>()
                .ForMember(dest => dest.CatalogId, opt => opt.MapFrom(src => src.CatalogId))
                .ForMember(dest => dest.ModelId, opt => opt.MapFrom(src => src.ModelId))
                .ForMember(dest => dest.NomenclatureId, opt => opt.MapFrom(src => src.NomenclatureId))
                .ForMember(dest => dest.SizeLine, opt => opt.MapFrom(src => src.SizeLine))
                .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.Size))
                .ForMember(dest => dest.ColorId, opt => opt.MapFrom(src => src.ColorId));
            
            CreateMap<CatalogItemFull, ItemPriceDescriptorDTO>()
                .ForMember(dest => dest.CatalogId, opt => opt.MapFrom(src => src.CatalogId))
                .ForMember(dest => dest.ModelId, opt => opt.MapFrom(src => src.ModelId))
                .ForMember(dest => dest.NomenclatureId, opt => opt.MapFrom(src => src.NomenclatureId))
                .ForMember(dest => dest.SizeLine, opt => opt.MapFrom(src => src.SizeLine))
                .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.Size))
                .ForMember(dest => dest.ColorId, opt => opt.MapFrom(src => src.ColorId))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.PriceInRub, opt => opt.MapFrom(src => src.PriceInRub))
                .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Discount));
            
            CreateMap<CatalogExtraCharge, CatalogExtraChargeDTO>();
            
              CreateMap<ModelPhoto1C, ModelPhotoDTO>()
                  .ForMember(dest => dest.ColorName, opt => opt.MapFrom(src => src.Color != null ? src.Color.Name : ""));
              
              CreateMap<CatalogItemForAdd, CatalogItemForAddDTO>()
                  .ForMember(dest => dest.CatalogId, opt => opt.MapFrom(src => src.CatalogId))
                  .ForMember(dest => dest.CatalogName, opt => opt.MapFrom(src => (string.IsNullOrEmpty(src.CatalogName) ? src.CatalogId.ToProductCatalog1CId().AsString() : src.CatalogName)))
                  .ForMember(dest => dest.Articul, opt => opt.MapFrom(src => src.Articul))
                  .ForMember(dest => dest.ModelId, opt => opt.MapFrom(src => src.ModelId))
                  .ForMember(dest => dest.ColorId, opt => opt.MapFrom(src => src.ColorId))
                  .ForMember(dest => dest.ColorName, opt => opt.MapFrom(src => src.ColorName))
                  .ForMember(dest => dest.ColorValue, opt => opt.MapFrom(src => src.ColorValue))
                  .ForMember(dest => dest.ColorPhoto, opt => opt.MapFrom(src => src.ColorPhoto))
                  .ForMember(dest => dest.NomenclatureId, opt => opt.MapFrom(src => src.NomenclatureId))
                  .ForMember(dest => dest.PartsCount, opt => opt.MapFrom(src => (src.PartsCount <= 0 ? 1 : (src.NomenclatureId > 0 ? 1 : src.PartsCount))))
                  .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                  .ForMember(dest => dest.SizeValue, opt => opt.MapFrom(src => (src.NomenclatureId > 0 ? src.Size : src.SizeLine)))
                  .ForMember(dest => dest.ModelPhoto, opt => opt.MapFrom(src => (string.IsNullOrEmpty(src.PhotoByColor) ? src.MainPhoto : src.PhotoByColor)));

              CreateMap<UpdateCatalogItemDescriptorDTO, CatalogItemDescriptor>()
                  .ForMember(dest => dest.Matherial, opt => opt.MapFrom(src => src.Material));

              CreateMap<CatalogItemFull, CatalogItemDTO>()
                  .ForMember(dest => dest.Material, opt => opt.MapFrom(src => src.MaterialName))
                  .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.RealSize))
                  .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src))
                  .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.ItemsCount))
                  .ForMember(dest => dest.RealAmount, opt => opt.MapFrom(src => src.ItemsCount / src.RealSize.Parts))
                  .ForMember(dest => dest.IsVisible, opt => opt.MapFrom(src => src.IsVisibleColor));

              CreateMap<CatalogItemBriefDTO, CatalogItemGroupNew>();
        }
    }
}