namespace Module.Catalogs.Business.Models {
    public class CatalogItemForAddDTO {
        public int CatalogId { get; set; }
        public string CatalogName { get; set; }
        public string Articul { get; set; }
        public int ModelId { get; set; }
        public int ColorId { get; set; }
        public string ColorName { get; set; }
        public string ColorValue { get; set; }
        public string ColorPhoto { get; set; }
        public int NomenclatureId { get; set; }
        public int PartsCount { get; set; }
        public int Amount { get; set; }
        public string SizeValue { get; set; }
        public string ModelPhoto { get; set; }
    }
}
