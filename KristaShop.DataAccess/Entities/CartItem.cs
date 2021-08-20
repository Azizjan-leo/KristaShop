#nullable enable
using System;
using KristaShop.Common.Enums;
using KristaShop.DataAccess.Configurations;
using KristaShop.DataAccess.Entities.DataFrom1C;
using KristaShop.DataAccess.Entities.Interfaces;

namespace KristaShop.DataAccess.Entities {
    /// <summary>
    /// Configuration file for this entity <see cref="CartItemConfiguration"/>
    /// </summary>
    public class CartItem : IUserCatalogNomenclature {
        public int Id { get; set; }
        public int UserId { get; set; }
        public CatalogType CatalogId { get; set; }
        public string Articul { get; set; }
        public int ModelId { get; set; }
        public int NomenclatureId { get; set; }
        public int ColorId { get; set; }
        public string SizeValue { get; set; }
        public double Price { get; set; }
        public double PriceInRub { get; set; }
        public int Amount { get; set; }
        public DateTime CreatedDate { get; set; }

        public User? User { get; set; }
        public Model? Model { get; set; }
        public Color? Color { get; set; }
        
        public CartItem() {
            Articul = "";
            SizeValue = "";
            CreatedDate = DateTime.Now;
        }
    }
}