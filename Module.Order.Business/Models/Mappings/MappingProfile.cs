using AutoMapper;
using KristaShop.Common.Enums;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.DataFor1C;
using KristaShop.DataAccess.Entities.DataFrom1C;
using KristaShop.DataAccess.Views;

namespace Module.Order.Business.Models.Mappings {
    public class MappingProfile : Profile {
        public MappingProfile() {
             CreateMap<CartItem1CDTO, OrderDetails>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CatalogId, opt => opt.MapFrom(src => src.CatalogId))
                .ForMember(dest => dest.NomenclatureId, opt => opt.MapFrom(src => src.NomenclatureId))
                .ForMember(dest => dest.ModelId, opt => opt.MapFrom(src => src.ModelId))
                .ForMember(dest => dest.ColorId, opt => opt.MapFrom(src => src.ColorId))
                .ForMember(dest => dest.SizeValue, opt => opt.MapFrom(src => src.SizeValue))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.PriceInRub, opt => opt.MapFrom(src => src.PriceInRub))
                .ForMember(dest => dest.Articul, opt => opt.MapFrom(src => src.Articul))
                .ForMember(dest => dest.ColorName, opt => opt.MapFrom(src => src.ColorName));


            CreateMap<CreateOrderDTO, KristaShop.DataAccess.Entities.DataFor1C.Order>()
                .ForMember(dest => dest.Details, opt => opt.Ignore());

            CreateMap<KristaShop.DataAccess.Entities.DataFor1C.Order, OrderDTO>();
            CreateMap<OrderDetailsFull, OrderDetailsDTO>()
                .ForMember(dest => dest.PhotoByColor, opt => opt.MapFrom(src => (string.IsNullOrEmpty(src.PhotoByColor) ? src.MainPhoto : src.PhotoByColor)))
                .ForMember(dest => dest.ColorValue, opt => opt.MapFrom(src => (string.IsNullOrEmpty(src.ColorValue) ? "#FFFFFF" : src.ColorValue)))
                .ForMember(dest => dest.ColorPhoto, opt => opt.MapFrom(src => src.ColorPhoto))
                .ForMember(dest => dest.SizeValue, opt => opt.MapFrom(src => src.Size.Value))
                .ForMember(dest => dest.PartsCount, opt => opt.MapFrom(src => src.Size.Parts))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Price * src.Amount))
                .ForMember(dest => dest.TotalPriceInRub, opt => opt.MapFrom(src => src.PriceInRub * src.Amount));
            
            CreateMap<OrderAdmin, OrderAdminDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.CreateDate))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.UserFullName))
                .ForMember(dest => dest.ManagerId, opt => opt.MapFrom(src => src.ManagerId))
                .ForMember(dest => dest.ManagerFullName, opt => opt.MapFrom(src => (string.IsNullOrEmpty(src.ManagerFullName) ? "---" : src.ManagerFullName)))
                .ForMember(dest => dest.IsProcessedPreorder, opt => opt.MapFrom(src => src.IsProcessedPreorder))
                .ForMember(dest => dest.IsProcessedRetail, opt => opt.MapFrom(src => src.IsProcessedRetail))
                .ForMember(dest => dest.TotalSum, opt => opt.MapFrom(src => src.TotalSum))
                .ForMember(dest => dest.PreorderTotalSum, opt => opt.MapFrom(src => src.PreorderTotalSum))
                .ForMember(dest => dest.RetailTotalSum, opt => opt.MapFrom(src => src.RetailTotalSum))
                .ForMember(dest => dest.PreorderAmount, opt => opt.MapFrom(src => src.PreorderAmount))
                .ForMember(dest => dest.RetailAmount, opt => opt.MapFrom(src => src.RetailAmount))
                .ForMember(dest => dest.CityId, opt => opt.MapFrom(src => src.CityId))
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => (src.CityId > 0 && !string.IsNullOrEmpty(src.CityName) ? src.CityName : "---")));

            CreateMap<ClientOrdersTotalsSqlView, ClientOrdersTotalsDTO>();

            CreateMap<RequestAdmin, RequestAdminDTO>();
            CreateMap<ManufactureAdmin, ManufactureAdminDTO>();
            CreateMap<ReservationAdmin, ReservationAdminDTO>();
            CreateMap<ShipingAdmin, ShipingAdminDTO>();

            CreateMap<InvoiceSql, InvoiceDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.UserFullName))
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.CityName))
                .ForMember(dest => dest.ManagerId, opt => opt.MapFrom(src => src.ManagerId))
                .ForMember(dest => dest.ManagerFullName, opt => opt.MapFrom(src => src.ManagerFullName))
                .ForMember(dest => dest.InvoiceClientTitle, opt => opt.MapFrom(src => src.InvoiceClientTitle))
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.CreateDate))
                .ForMember(dest => dest.InvoiceNum, opt => opt.MapFrom(src => src.InvoiceNum))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => (src.Currency.Trim().ToLower() == "usd" ? InvoiceCurrency.USD : InvoiceCurrency.RUB)))
                .ForMember(dest => dest.PrePay, opt => opt.MapFrom(src => src.PrePay))
                .ForMember(dest => dest.TotalPay, opt => opt.MapFrom(src => src.TotPay))
                .ForMember(dest => dest.ExchangeRate, opt => opt.MapFrom(src => (src.ExchangeRate <= 0.0d ? 1.0d : src.ExchangeRate)))
                .ForMember(dest => dest.WasPayed, opt => opt.MapFrom(src => src.WasPayed))
                .ForMember(dest => dest.IsPrePay, opt => opt.MapFrom(src => src.IsPrepay > 0));

            CreateMap<InvoiceLineSql, InvoiceLineDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.InvoiceId, opt => opt.MapFrom(src => src.InvoiceId))
                .ForMember(dest => dest.IsProductLine, opt => opt.MapFrom(src => !src.IsProductLine))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Articul, opt => opt.MapFrom(src => src.Articul))
                .ForMember(dest => dest.ModelId, opt => opt.MapFrom(src => src.ModelId))
                .ForMember(dest => dest.ColorId, opt => opt.MapFrom(src => src.ColorId))
                .ForMember(dest => dest.ColorName, opt => opt.MapFrom(src => src.ColorName))
                .ForMember(dest => dest.ColorValue, opt => opt.MapFrom(src => src.ColorValue))
                .ForMember(dest => dest.ColorPhoto, opt => opt.MapFrom(src => src.ColorPhoto))
                .ForMember(dest => dest.PartsCount, opt => opt.MapFrom(src => (src.PartsCount <= 0 ? 1 : (!string.IsNullOrEmpty(src.Size) ? 1 : src.PartsCount))))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.SizeValue, opt => opt.MapFrom(src => (!string.IsNullOrEmpty(src.Size) ? src.Size : src.SizeLine)))
                .ForMember(dest => dest.MainPhoto, opt => opt.MapFrom(src => (string.IsNullOrEmpty(src.PhotoByColor) ? src.MainPhoto : src.PhotoByColor)));
            
              CreateMap<CartItem, CartItem1CDTO>()
                .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User != null ? src.User.FullName : ""))
                .ForMember(dest => dest.ManagerId, opt => opt.MapFrom(src => src.User != null ? src.User.ManagerId : 0))
                .ForMember(dest => dest.ManagerName, opt => opt.MapFrom(src => src.User != null && src.User.Manager != null ? src.User.Manager.Name : ""))
                .ForMember(dest => dest.CityId, opt => opt.MapFrom(src => src.User != null && src.User.City != null ? src.User.City.Id : 0))
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.User != null && src.User.City != null ? src.User.City.Name : ""))
                .ForMember(dest => dest.CatalogName, opt => opt.MapFrom(src => src.CatalogId.AsString()))
                .ForMember(dest => dest.ColorName, opt => opt.MapFrom(src => src.Color.Name))
                .ForMember(dest => dest.ColorValue, opt => opt.MapFrom(src => src.Color.Group != null ? src.Color.Group.Hex : ""))
                .ForMember(dest => dest.ColorPhoto, opt => opt.MapFrom(src => ""))
                .ForMember(dest => dest.PartsCount, opt => opt.MapFrom(src => (src.Model.Parts > 0 ? (src.NomenclatureId > 0 ? 1 : src.Model.Parts) : 1)))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Price * src.Amount))
                .ForMember(dest => dest.TotalPriceInRub, opt => opt.MapFrom(src => src.PriceInRub * src.Amount))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Model.Descriptor.MainPhoto));
            
            CreateMap<CartItemSqlView, CartItem1CDTO>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => (string.IsNullOrEmpty(src.PhotoByColor) ? src.MainPhoto : src.PhotoByColor)))
                .ForMember(dest => dest.ColorValue, opt => opt.MapFrom(src => (string.IsNullOrEmpty(src.ColorValue) ? "#FFFFFF" : src.ColorValue)))
                .ForMember(dest => dest.PartsCount, opt => opt.MapFrom(src => (src.PartsCount > 0 ? (src.NomenclatureId > 0 ? 1 : src.PartsCount) : 1)))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Price * src.Amount))
                .ForMember(dest => dest.TotalPriceInRub, opt => opt.MapFrom(src => src.PriceInRub * src.Amount));

            CreateMap<CartItem1CDTO, CartItem>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.CatalogId, opt => opt.MapFrom(src => src.CatalogId))
                .ForMember(dest => dest.Articul, opt => opt.MapFrom(src => src.Articul))
                .ForMember(dest => dest.ModelId, opt => opt.MapFrom(src => src.ModelId))
                .ForMember(dest => dest.NomenclatureId, opt => opt.MapFrom(src => src.NomenclatureId))
                .ForMember(dest => dest.ColorId, opt => opt.MapFrom(src => src.ColorId))
                .ForMember(dest => dest.SizeValue, opt => opt.MapFrom(src => src.SizeValue))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.PriceInRub, opt => opt.MapFrom(src => src.PriceInRub));
            
            CreateMap<UserCartTotals, UserCartTotalsDTO>();
            
            CreateMap<OrderHistorySqlView, OrderHistoryItemDTO>()
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.OrderDate))
                .ForMember(dest => dest.ColorValue, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.ColorValue) ? "#FFFFFF" : src.ColorValue));

            CreateMap<OrderDetailsFull, UnprocessedOrderItemDTO>()
                .ForMember(dest => dest.ColorValue, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.ColorValue) ? "#FFFFFF" : src.ColorValue));
            
            CreateMap<MoneyDocumentItemSqlView, MoneyDocumentItemDTO>();
            CreateMap<MoneyDocumentsTotalSqlView, MoneyDocumentsTotalDTO>();

            CreateMap<ClientManufacturingItemSqlView, ManufacturingItemDTO>();
            CreateMap<MoneyDocumentSqlView, MoneyDocumentDTO>();
            
            CreateMap<ClientRequestItemSqlView, RequestsItemDTO>()
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.RequestDate));

            CreateMap<OrderHistorySqlView, RequestsItemDTO>();

            CreateMap<ClientReservationItemSqlView, ReservationsItemDTO>()
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.ReservationDate));
            
            CreateMap<ShipmentsSqlView, ShipmentsItemDTO>()
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.SaleDate));
        }
    }
}