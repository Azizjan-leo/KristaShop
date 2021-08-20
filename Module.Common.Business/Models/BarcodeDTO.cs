using KristaShop.Common.Models.Structs;

namespace Module.Common.Business.Models {
    public class BarcodeDTO {
        public SizeValue Size { get; set; }
        public int ColorId { get; set; }
        public string Barcode { get; set; }
    }
}