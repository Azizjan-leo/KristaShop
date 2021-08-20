using System;

namespace KristaShop.DataAccess.Entities.DataFrom1C {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.DataFrom1C.CartItemSqlViewConfiguration"/>
    /// </summary>
    public class CartItemSqlView {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public int ManagerId { get; set; }
        public string ManagerName { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int CatalogId { get; set; }
        public string CatalogName { get; set; }
        public string Articul { get; set; }
        public int ModelId { get; set; }
        public int NomenclatureId { get; set; }
        public string PhotoByColor { get; set; }
        public string MainPhoto { get; set; }
        public int ColorId { get; set; }
        public string ColorName { get; set; }
        public string ColorPhoto { get; set; }
        public string ColorValue { get; set; }
        public string SizeValue { get; set; }
        public double Price { get; set; }
        public double PriceInRub { get; set; }
        public int Amount { get; set; }
        public int PartsCount { get; set; }
        public DateTime CreatedDate { get; set; }       
    }
}
