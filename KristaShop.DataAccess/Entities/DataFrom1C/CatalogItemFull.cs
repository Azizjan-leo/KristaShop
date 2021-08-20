using KristaShop.Common.Models.Structs;

namespace KristaShop.DataAccess.Entities.DataFrom1C {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.DataFrom1C.CatalogItemFullConfiguration"/>
    /// </summary>
    public class CatalogItemFull {
        public int Id { get; set; }
        public int ModelId { get; set; }
        public int NomenclatureId { get; set; }
        public int ItemsCount { get; set; }
        public int CatalogId { get; set; }
        public double Price { get; set; }
        public double PriceInRub { get; set; }
        public int Discount { get; set; }
        public string ModelName { get; set; }
        public string Articul { get; set; }
        public int Status { get; set; }
        public int PartsCount { get; set; }
        public double Weight { get; set; }
        public string SizeLine { get; set; }
        public string Size { get; set; }
        public int MaterialId { get; set; }
        public string MaterialName { get; set; }
        public int ColorId { get; set; }
        public int ColorGroupId { get; set; }
        public string ColorGroupName { get; set; }
        public string ColorName { get; set; }
        public string ColorValue { get; set; }
        public string ColorPhoto { get; set; }
        public bool IsVisibleColor { get; set; }
        public SizeValue RealSize => new(Size, SizeLine);
    }
}
