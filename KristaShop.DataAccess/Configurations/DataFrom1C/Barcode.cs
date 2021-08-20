using KristaShop.DataAccess.Entities.DataFrom1C;

namespace KristaShop.DataAccess.Configurations.DataFrom1C {
    public class Barcode {
        /// <summary>
        /// Configuration file for this entity <see cref="BarcodeConfiguration"/>
        /// </summary>
        public int Id { get; set; }
        public string Value { get; set; }
        public int ModelId { get; set; }
        public int ColorId { get; set; }
        public string SizeValue { get; set; }

        public Model Model { get; set; }
        public Color Color { get; set; }
    }
}