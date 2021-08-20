using KristaShop.DataAccess.Entities.DataFrom1C;

namespace KristaShop.DataAccess.Entities {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.ModelCategoryConfiguration"/>
    /// </summary>
    public class ModelCategory {
        public int Id { get; set; }
        public int ModelId { get; set; }
        public int CategoryId { get; set; }
        public Model Model { get; set; }
        public Category Category { get; set; }
    }
}