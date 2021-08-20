namespace Module.Order.Business.Models {
    public class OrderDetailsDTO {
        public int Id { get; set; }
        public int CatalogId { get; set; }
        public string CatalogName { get; set; }   
        public string Articul { get; set; }
        public string PhotoByColor { get; set; }
        public string MainPhoto { get; set; }
        public int ModelId { get; set; }
        public int NomenclatureId { get; set; }
        public int ColorId { get; set; }
        public string ColorName { get; set; }
        public string ColorValue { get; set; }
        public string ColorPhoto { get; set; }
        public string SizeValue { get; set; }
        public int Amount { get; set; }
        public int PartsCount { get; set; }
        public double Price { get; set; }
        public double PriceInRub { get; set; }
        public double TotalPrice { get; set; }
        public double TotalPriceInRub { get; set; }
        public int CollectionPrepayPercent { get; set; }
        public double PrepayPercent { get; set; }
        public string ItemGroupingKey => $"{Articul}_{SizeValue}";

        public OrderDetailsDTO() 
            : base() {

        }

        public OrderDetailsDTO(OrderDetailsDTO value) {
            Id = value.Id;
            CatalogId = value.CatalogId;
            CatalogName = value.CatalogName;
            Articul = value.Articul;
            PhotoByColor = value.PhotoByColor;
            ModelId = value.ModelId;
            NomenclatureId = value.NomenclatureId;
            ColorId = value.ColorId;
            ColorName = value.ColorName;
            ColorValue = value.ColorValue;
            ColorPhoto = value.ColorPhoto;
            SizeValue = value.SizeValue;
            Amount = value.Amount;
            PartsCount = value.PartsCount;
            Price = value.Price;
            PriceInRub = value.PriceInRub;
            TotalPrice = value.TotalPrice;
            TotalPriceInRub = value.TotalPriceInRub;
        }
    }
}