namespace Module.Catalogs.Business.Models {
    public class ItemPriceDescriptorDTO {
        public int CatalogId { get; set; }
        public int ModelId { get; set; }
        public int NomenclatureId { get; set; }
        public string SizeLine { get; set; }
        public string Size { get; set; }
        public int ColorId { get; set; }
        public double Price { get; set; }
        public double PriceInRub { get; set; }
        public double Discount { get; set; }

        public bool IsLine() {
            return NomenclatureId == 0;
        }
    }
}