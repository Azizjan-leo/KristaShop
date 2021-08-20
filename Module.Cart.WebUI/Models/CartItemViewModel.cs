using System.ComponentModel.DataAnnotations;
using KristaShop.Common.Enums;

namespace Module.Cart.WebUI.Models {
    public class CartItemViewModel {
        public int UserId { get; set; }
        public CatalogType CatalogId { get; set; }
        public string Articul { get; set; }
        public int ModelId { get; set; }
        public int NomenclatureId { get; set; }
        public int ColorId { get; set; }
        public string SizeValue { get; set; }
        [DataType(DataType.Currency)] 
        public double Price { get; set; }
        public double PriceInRub { get; set; }
        public int Amount { get; set; }
    }
}