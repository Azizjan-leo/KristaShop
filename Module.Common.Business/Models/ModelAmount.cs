using KristaShop.Common.Models.Structs;

namespace Module.Common.Business.Models {
    public class ModelAmount {
        public int ModelId { get; set; }
        public SizeValue Size { get; set; }
        public int ColorId { get; set; }
        public int Amount { get; set; }

        public ModelAmount() {
            
        }

        public ModelAmount(int modelId, SizeValue size, int colorId, int amount) {
            ModelId = modelId;
            Size = size;
            ColorId = colorId;
            Amount = amount;
        }
    }
}