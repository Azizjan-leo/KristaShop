using KristaShop.Common.Models.Structs;

namespace KristaShop.Common.Interfaces.Settings {
    public interface IPreorderParts {
        CatalogItemsPartition PreorderParts { get; set; }
        int FrontCatalogOnPageCount { get; set; }
    }
}
