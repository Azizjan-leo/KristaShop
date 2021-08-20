namespace KristaShop.Common.Models.DTOs{
    public class BarcodeAmountDTO {
        public string Barcode { get; set; }
        public int Amount { get; set; }

        public BarcodeAmountDTO() { }
        
        public BarcodeAmountDTO(string barcode, int amount) {
            Barcode = barcode;
            Amount = amount;
        }
    }
}
