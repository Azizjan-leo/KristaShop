namespace KristaShop.DataAccess.Entities.DataFrom1C {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.DataFrom1C.StorehouseRestsConfiguration"/>
    /// </summary>
    public class StorehouseRests {
        public int Id { get; set; }
        public int ModelId { get; set; }
        public int NomenclatureId { get; set; }
        public int ColorId { get; set; }
        public int Amount { get; set; }
        public int StorehouseId { get; set; }
        public string StorehouseName { get; set; }
        public bool StorehouseIsCollective { get; set; }
        public int StorehousePriority { get; set; }
        public bool IsLine { get; set; }
    }
}
