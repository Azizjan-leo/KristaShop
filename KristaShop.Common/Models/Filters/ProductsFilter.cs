using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KristaShop.Common.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KristaShop.Common.Models.Filters {
    public class ProductsFilter {
        public CatalogType CatalogId { get; set; }
        public CatalogOrderDir OrderDirection { get; set; }
        public CatalogOrderType OrderType { get; set; }
        public string[] Articuls { get; set; }
        public bool IncludeDescription { get; set; }
        public bool ShowHiddenModels { get; set; }
        public bool HideEmptySlots { get; set; }

        public virtual bool AllowFilterByCatalog => CatalogId != CatalogType.All;

        public ProductsFilter() {
            CatalogId = CatalogType.All;
            OrderDirection = CatalogOrderDir.Asc;
            OrderType = CatalogOrderType.OrderByPosition;
            Articuls = null;
            IncludeDescription = false;
            ShowHiddenModels = true;
            HideEmptySlots = true;
        }
    }

    public class ProductsFilterExtended : ProductsFilter {
        public string Articul { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public IEnumerable<int> CatalogIds { get; set; }
        public IEnumerable<int> CategoriesIds { get; set; }
        public IEnumerable<int> ColorIds { get; set; }
        public IEnumerable<string> Sizes { get; set; }
        public IEnumerable<string> SizeLines { get; set; }
        public bool ExcludeHiddenByColors { get; set; }
        public bool IncludeCategoriesMap { get; set; }
        public bool IncludeCatalogsMap { get; set; }
        public bool IncludePhotosList { get; set; }
        public bool IncludeItemsDescriptions { get; set; }
        public bool IncludeWithoutCatalog { get; set; }

        public override bool AllowFilterByCatalog => CatalogId != CatalogType.All && (CatalogIds == null || !CatalogIds.Any());

        public ProductsFilterExtended() {
            ShowHiddenModels = false;
            Articul = string.Empty;
            MinPrice = 0.0;
            MaxPrice = 0.0;
            CatalogIds = null;
            CategoriesIds = null;
            ColorIds = null;
            Sizes = null;
            SizeLines = null;
            ExcludeHiddenByColors = true;
            IncludeCategoriesMap = false;
            IncludeWithoutCatalog = false;
        }

        public void SetCatalogIds(List<int> catalogIds) {
            if (CatalogId != CatalogType.All) {
                catalogIds.RemoveAll(x => x != (int) CatalogId);
            }
            if (!catalogIds.Any()) {
                catalogIds = new List<int>() { (int)CatalogType.Open };
            }
            CatalogIds = catalogIds;
        }
    }
}