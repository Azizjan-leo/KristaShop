using KristaShop.DataAccess.Configurations.DataFrom1C;

namespace KristaShop.DataAccess.Entities.DataFrom1C {
    /// <summary>
    /// Configuration file for this entity <see cref="ColorGroupConfiguration"/>
    /// </summary>
    public class ColorGroup {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Hex { get; set; }
    }
}