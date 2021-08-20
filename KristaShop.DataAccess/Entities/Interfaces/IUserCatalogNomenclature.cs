using KristaShop.Common.Enums;

namespace KristaShop.DataAccess.Entities.Interfaces {
    public interface IUserCatalogNomenclature : IUserNomenclature {
        public CatalogType CatalogId { get; set; }
    }
}