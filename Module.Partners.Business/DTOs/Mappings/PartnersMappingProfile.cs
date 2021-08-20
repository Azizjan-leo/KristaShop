using System.Linq;
using AutoMapper;
using KristaShop.Common.Extensions;
using KristaShop.Common.Interfaces.Models;
using KristaShop.Common.Models.Structs;
using KristaShop.DataAccess.Entities.DataFrom1C;
using KristaShop.DataAccess.Entities.Partners;
using KristaShop.DataAccess.Views.Partners;
using Module.Common.Business.Models;

namespace Module.Partners.Business.DTOs.Mappings {
    public class PartnersMappingProfile : Profile {
        public PartnersMappingProfile() {
            CreateMap<PartnerSqlView, PartnerDTO>();
            CreateMap<PartnerStorehouseItemSqlView, PartnerStorehouseItemDTO>();
            CreateMap<PartnerStorehouseHistoryItemSqlView, PartnerStorehouseItemDTO>();
            CreateMap<PartnershipRequestSqlView, PartnershipRequestDTO>(); 

            CreateMap<DocumentItem, DocumentItemDTO>();
            CreateMap<DocumentItemDTO, DocumentItem>();
            CreateMap<PartnerStorehouseItem, DocumentItemDTO>();
            CreateMap<IBarcodesCountableCatalogItem, DocumentItemDTO>();

            CreateMap<Document, DocumentDTO<DocumentItemDetailedDTO>>()
                .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.Partner != null && src.Partner.User != null ? src.Partner.User.FullName : ""))
                .ForMember(dest => dest.Totals, opt => opt.MapFrom(src => src.GetTotals()))
                .ForMember(dest => dest.NextActionName, opt => opt.MapFrom(src => src.GetNextActionName()))
                .Include<RevisionDocument, RevisionDocumentDTO<DocumentItemDetailedDTO>>()
                .Include<RevisionDeficiencyDocument, RevisionDeficiencyDocumentDTO<DocumentItemDetailedDTO>>()
                .Include<RevisionExcessDocument, RevisionExcessDocumentDTO<DocumentItemDetailedDTO>>()
                .Include<MoneyDocument, MoneyDocumentDTO<DocumentItemDetailedDTO>>()
                .Include<PaymentDocument, PaymentDocumentDTO<DocumentItemDetailedDTO>>()
                .Include<IncomeDocument, IncomeDocumentDTO<DocumentItemDetailedDTO>>()
                .Include<SellingRequestDocument, SellingRequestDocumentDTO<DocumentItemDetailedDTO>>();

            CreateMap<RevisionDocument, RevisionDocumentDTO<DocumentItemDetailedDTO>>();
            CreateMap<RevisionDeficiencyDocument, RevisionDeficiencyDocumentDTO<DocumentItemDetailedDTO>>();
            CreateMap<RevisionExcessDocument, RevisionExcessDocumentDTO<DocumentItemDetailedDTO>>();
            CreateMap<MoneyDocument, MoneyDocumentDTO<DocumentItemDetailedDTO>>();
            CreateMap<PaymentDocument, PaymentDocumentDTO<DocumentItemDetailedDTO>>();
            CreateMap<IncomeDocument, IncomeDocumentDTO<DocumentItemDetailedDTO>>();
            CreateMap<SellingRequestDocument, SellingRequestDocumentDTO<DocumentItemDetailedDTO>>();

            CreateMap<DocumentItem, DocumentItemDetailedDTO>()
                .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.Model == null ? src.Size : new SizeValue(src.Size.Value, src.Model.SizeLine)))
                .ForMember(dest => dest.ColorName, opt => opt.MapFrom(src => src.Color == null ? default : src.Color.Name))
                .ForMember(dest => dest.ColorImage, opt => opt.MapFrom(src => ""))
                .ForMember(dest => dest.ColorCode, opt => opt.MapFrom(src => src.Color == null || src.Color.Group == null ? default : src.Color.Group.Hex))
                .ForMember(dest => dest.ModelName, opt => opt.MapFrom(src => src.Model == null ? default : src.Model.Name))
                .ForMember(dest => dest.MainPhoto, opt => opt.MapFrom(src => src.Model == null || src.Model.Descriptor == null ? default : src.Model.Descriptor.MainPhoto))
                .ForMember(dest => dest.FromDocumentName, opt => opt.MapFrom(src => src.FromDocument == null ? default : src.FromDocument.Name));


            CreateMap<PartnerStorehouseHistoryItemSqlView, DocumentItem>();
            CreateMap<PartnerStorehouseHistoryItemSqlView, DocumentItemDetailedDTO>();
            CreateMap<PartnerStorehouseHistoryItemSqlView, DocumentMoneyItemsDetailedDTO>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.CreateDate))
                .ForMember(dest => dest.DocumentType, opt => opt.MapFrom(src => src.MovementType.GetDisplayName()));
            
              
            CreateMap<Document, DocumentDTO<ModelGroupedDTO>>()
                .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.Partner != null && src.Partner.User != null ? src.Partner.User.FullName : ""))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items.GroupBy(x=>x.GetModelGroupingKey())))
                .ForMember(dest => dest.Totals, opt => opt.MapFrom(src => src.GetTotals()))
                .ForMember(dest => dest.NextActionName, opt => opt.MapFrom(src => src.GetNextActionName()))
                .Include<RevisionDocument, RevisionDocumentDTO<ModelGroupedDTO>>()
                .Include<RevisionDeficiencyDocument, RevisionDeficiencyDocumentDTO<ModelGroupedDTO>>()
                .Include<RevisionExcessDocument, RevisionExcessDocumentDTO<ModelGroupedDTO>>()
                .Include<MoneyDocument, MoneyDocumentDTO<ModelGroupedDTO>>()
                .Include<PaymentDocument, PaymentDocumentDTO<ModelGroupedDTO>>()
                .Include<IncomeDocument, IncomeDocumentDTO<ModelGroupedDTO>>()
                .Include<SellingRequestDocument, SellingRequestDocumentDTO<ModelGroupedDTO>>();
            
            CreateMap<RevisionDocument, RevisionDocumentDTO<ModelGroupedDTO>>();
            CreateMap<RevisionDeficiencyDocument, RevisionDeficiencyDocumentDTO<ModelGroupedDTO>>();
            CreateMap<RevisionExcessDocument, RevisionExcessDocumentDTO<ModelGroupedDTO>>();
            CreateMap<MoneyDocument, MoneyDocumentDTO<ModelGroupedDTO>>();
            CreateMap<PaymentDocument, PaymentDocumentDTO<ModelGroupedDTO>>();
            CreateMap<IncomeDocument, IncomeDocumentDTO<ModelGroupedDTO>>();
            CreateMap<SellingRequestDocument, SellingRequestDocumentDTO<ModelGroupedDTO>>();

            CreateMap<IGrouping<string, DocumentItem>, ModelGroupedDTO>()
                .ForMember(dest => dest.ModelKey, opt => opt.MapFrom(src => src.Key))
                .ForMember(dest => dest.ModelInfo, opt => opt.MapFrom(src => src.First()))
                .ForMember(dest => dest.Colors, opt => opt.MapFrom(src => src.GroupBy(c => c.ColorId).Select(c => c.First().Color)))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.Sum(x=>x.Amount)))
                .ForMember(dest => dest.TotalAmountByColor, opt => opt.MapFrom(src => src.GroupBy(c => c.ColorId).ToDictionary(k => k.Key, v => v.Sum(c => c.Amount))))
                .ForMember(dest => dest.TotalSum, opt => opt.MapFrom(src => src.Sum(c => c.Amount * c.Price)))
                .ForMember(dest => dest.TotalSumByColor, opt => opt.MapFrom(src => src.GroupBy(c => c.ColorId).ToDictionary(k => k.Key, v => v.Sum(c => c.Amount * c.Price))))
                .ForMember(dest => dest.SizesInfo, opt => opt.MapFrom(src => new SizesAmountsDTO {
                    Values = src.Select(c => c.Size.Value).Distinct().OrderBy(c => c).ToList(),
                    SizeColorAmount = src.GroupBy(c => $"{c.Size.Value}_{c.ColorId}").ToDictionary(k => k.Key, v => v.Sum(c => c.Amount)),
                    TotalAmountBySize = src.GroupBy(c => c.Size.Value).ToDictionary(k => k.Key, v => v.Sum(c => c.Amount))
                }));

            CreateMap<Color, ColorDTO>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Group == null ? default : src.Group.Hex));
            
            CreateMap<DocumentItem, ModelInfoDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Model == null ? default : src.Model.Name))
                .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.Model == null ? src.Size : new SizeValue(src.Model.SizeLine)))
                .ForMember(dest => dest.MainPhoto, opt => opt.MapFrom(src => src.Model == null || src.Model.Descriptor == null ? default : src.Model.Descriptor.MainPhoto));

            CreateMap<IGrouping<string, PartnerStorehouseItemSqlView>, ModelGroupedDTO>()
                .ForMember(dest => dest.ModelKey, opt => opt.MapFrom(src => src.Key))
                .ForMember(dest => dest.ModelInfo, opt => opt.MapFrom(src => src.First()))
                .ForMember(dest => dest.Colors, opt => opt.MapFrom(src => src.GroupBy(c => c.ColorId).Select(c => new ColorDTO {Id = c.First().ColorId, Name = c.First().ColorName, Code = c.First().ColorCode, Image = c.First().ColorImage})))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.Sum(x=>x.Amount)))
                .ForMember(dest => dest.TotalAmountByColor, opt => opt.MapFrom(src => src.GroupBy(c => c.ColorId).ToDictionary(k => k.Key, v => v.Sum(c => c.Amount))))
                .ForMember(dest => dest.TotalSum, opt => opt.MapFrom(src => src.Sum(c => c.Amount * c.Price)))
                .ForMember(dest => dest.TotalSumByColor, opt => opt.MapFrom(src => src.GroupBy(c => c.ColorId).ToDictionary(k => k.Key, v => v.Sum(c => c.Amount * c.Price))))
                .ForMember(dest => dest.SizesInfo, opt => opt.MapFrom(src => new SizesAmountsDTO {
                    Values = src.Select(c => c.Size.Value).Distinct().OrderBy(c => c).ToList(),
                    SizeColorAmount = src.GroupBy(c => $"{c.Size.Value}_{c.ColorId}").ToDictionary(k => k.Key, v => v.Sum(c => c.Amount)),
                    TotalAmountBySize = src.GroupBy(c => c.Size.Value).ToDictionary(k => k.Key, v => v.Sum(c => c.Amount))
                }))
                .ForMember(dest => dest.Barcodes, opt => opt.MapFrom(src => src.SelectMany(x => x.Barcodes, (x, b) => new BarcodeDTO { Barcode = b, ColorId = x.ColorId, Size = x.Size})));
            CreateMap<PartnerStorehouseItemSqlView, ModelInfoDTO>();

            CreateMap<DocumentMovementAmounts, DocumentMovementAmountsDTO>();

            CreateMap<PartnerStorehouseItemSqlView, DocumentItemDTO>();
        }
    }
}
