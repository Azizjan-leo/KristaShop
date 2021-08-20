namespace KristaShop.Common.Enums {
    public enum CatalogOrderType {
        OrderByPosition = 0,
        OrderByPrice,
        OrderByDate,
        OrderRandom
    }

    public enum CatalogOrderDir {
        Asc = 0,
        Desc
    }

    public static class CatalogOrderDirExtension {
        public static CatalogOrderDir Invert(this CatalogOrderDir direction) {
            return direction == CatalogOrderDir.Asc ? CatalogOrderDir.Desc : CatalogOrderDir.Asc;
        }
    }
}
