namespace KristaShop.DataAccess.Entities.DataFrom1C {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.DataFrom1C.CatalogItem1CAmountConfiguration"/>
    /// </summary>
    public class CatalogItem1CAmount {
        public int Id { get; set; }
        public int ModelId { get; set; }
        public int ColorId { get; set; }
        public string SizeValue { get; set; }
        public int NomenclatureId { get; set; }
        public int Amount { get; set; }
        public int CatalogId { get; set; }
        public double Price { get; set; }
        public double PriceInRub { get; set; }
    }
}
