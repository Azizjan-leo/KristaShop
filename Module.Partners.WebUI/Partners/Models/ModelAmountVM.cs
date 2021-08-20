namespace Module.Partners.WebUI.Partners.Models {
    public class ModelAmountVM {
        public int ModelId { get; set; }
        public string Size { get; set; }
        public int ColorId { get; set; }
        public int Amount { get; set; }

        public ModelAmountVM() {
            
        }

        public ModelAmountVM(int modelId, string size, int colorId, int amount) {
            ModelId = modelId;
            Size = size;
            ColorId = colorId;
            Amount = amount;
        }
    }
}