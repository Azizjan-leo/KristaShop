namespace Module.Cart.WebUI.Models {
    public class CartUpdateResponse {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int PartsCount { get; set; }

        public CartUpdateResponse(bool success, string message, int partsCount) {
            Success = success;
            Message = message;
            PartsCount = partsCount;
        }
    }
}