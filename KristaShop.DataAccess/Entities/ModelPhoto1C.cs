#nullable enable
using KristaShop.DataAccess.Configurations;
using KristaShop.DataAccess.Entities.DataFrom1C;

namespace KristaShop.DataAccess.Entities {
    /// <summary>
    /// Configuration file for this entity <see cref="ModelPhoto1CConfiguration"/>
    /// </summary>
    public class ModelPhoto1C {
        public int Id { get; set; }
        public string Articul { get; set; }
        public string PhotoPath { get; set; }
        public string OldPhotoPath { get; set; }
        public int? ColorId { get; set; }
        public int Order { get; set; }
        public Color? Color { get; set; }

        public ModelPhoto1C() {
            Articul = "";
            PhotoPath = "";
            OldPhotoPath = "";
        }

        public ModelPhoto1C(string articul, string photoPath, int order) {
            Articul = articul;
            PhotoPath = photoPath;
            Order = order;
            OldPhotoPath = "";
        }
    }
}