using KristaShop.Common.Models.Structs;

namespace Module.Common.Business.Models {
    public class ModelInfoDTO {
        public string Articul { get; set; }
        public int ModelId { get; set; }
        public string Name { get; set; }
        public SizeValue Size { get; set; }
        public string MainPhoto { get; set; }
        public double Price { get; set; }
        public double PriceInRub { get; set; }
    }
}