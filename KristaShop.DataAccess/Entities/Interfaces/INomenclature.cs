using KristaShop.DataAccess.Entities.DataFrom1C;

namespace KristaShop.DataAccess.Entities.Interfaces {
    public interface INomenclature {
        public int ModelId { get; set; }
        public int ColorId { get; set; }
        public Model? Model { get; set; }
        public Color? Color { get; set; }
    }
}