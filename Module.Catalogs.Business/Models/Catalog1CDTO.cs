using KristaShop.Common.Enums;

namespace Module.Catalogs.Business.Models {
    public class Catalog1CDTO {
        public int Id { get; set; }
        public string Name { get; set; }

        public Catalog1CDTO() { }

        public Catalog1CDTO(CatalogType id) {
            Id = (int) id;
            Name = id.AsString();
        }

        public static Catalog1CDTO GetOpenCatalog() {
            return new Catalog1CDTO(CatalogType.Open);
        }
    }
}