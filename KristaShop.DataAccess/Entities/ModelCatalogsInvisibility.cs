using KristaShop.Common.Enums;

namespace KristaShop.DataAccess.Entities
{
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.ModelCatalogsInvisibilityConfiguration"/>
    /// </summary>
    public class ModelCatalogsInvisibility {
        public int Id { get; set; }
        public string Articul { get; set; }
        public CatalogType CatalogId { get; set; }

        public ModelCatalogsInvisibility() { }

        public ModelCatalogsInvisibility(string articul, CatalogType catalogId) {
            Articul = articul;
            CatalogId = catalogId;
        }
    }
}