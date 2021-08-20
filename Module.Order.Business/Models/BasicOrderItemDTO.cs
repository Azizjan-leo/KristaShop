using System;
using KristaShop.Common.Models.Structs;

namespace Module.Order.Business.Models {
    public class BasicOrderItemDTO : ICloneable {
        public int Id { get; set; }
        public string Articul { get; set; }
        public string MainPhoto { get; set; }
        public int ModelId { get; set; }
        public string ModelName { get; set; }
        public int NomenclatureId { get; set; }
        public int ColorId { get; set; }
        public string ColorName { get; set; }
        public string ColorValue { get; set; }
        public string ColorPhoto { get; set; }
        public SizeValue Size { get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }
        public double PriceInRub { get; set; }
        public double TotalPrice => Price * Amount;
        public double TotalPriceInRub => PriceInRub * Amount;
        public int CollectionId { get; set; }
        public string CollectionName { get; set; }
        public int CollectionPrepayPercent { get; set; }
        public double PrepayPercent { get; set; }
        public DateTime CreateDate { get; set; }
        public virtual string ItemStatus => string.Empty;
        public string ModelKey => $"{Articul}_{Size.Value}_{ColorName}";
        public string ItemGroupingKey => $"{Articul}_{Size.Value}";
        public string ColorCode => ColorValue;
        public object Clone() {
            return MemberwiseClone();
        }
    }
}