using KristaShop.Common.Enums;

namespace Module.Order.Business.Models {
    public class CartItem1CDTO {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public int ManagerId { get; set; }
        public string ManagerName { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public CatalogType CatalogId { get; set; }
        public string CatalogName { get; set; }      
        public string Articul { get; set; }
        public string Image { get; set; }
        public int ModelId { get; set; }
        public int NomenclatureId { get; set; }
        public int ColorId { get; set; }
        public string ColorName { get; set; }
        public string ColorValue { get; set; }
        public string MainPhoto { get; set; }
        public string ColorPhoto { get; set; }
        public string SizeValue { get; set; }
        public int Amount { get; set; }
        public int PartsCount { get; set; }
        public double Price { get; set; }
        public double PriceInRub { get; set; }
        public double TotalPrice { get; set; }
        public double TotalPriceInRub { get; set; }

        public bool IsPreorder => CatalogId == CatalogType.Preorder;
        public string ItemGroupingKey => $"{Articul}_{SizeValue}";
    }

}