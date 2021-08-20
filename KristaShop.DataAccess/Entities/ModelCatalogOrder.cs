using KristaShop.Common.Enums;
using KristaShop.DataAccess.Configurations;

namespace KristaShop.DataAccess.Entities {
    /// <summary>
    /// Configuration file for this entity <see cref="ModelCatalogOrderConfiguration"/>
    /// </summary>
    public class ModelCatalogOrder {
        public string Articul { get; set; }
        public CatalogType CatalogId { get; set; }
        public int Order { get; set; }

        public ModelCatalogOrder() {
        }

        public ModelCatalogOrder(string articul, CatalogType catalogId) {
            Articul = articul;
            CatalogId = catalogId;
        }
    }
}