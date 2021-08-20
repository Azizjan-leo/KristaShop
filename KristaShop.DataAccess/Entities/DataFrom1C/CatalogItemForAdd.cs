using System;
using System.Collections.Generic;
using System.Text;

namespace KristaShop.DataAccess.Entities.DataFrom1C {
    public class CatalogItemForAdd {
        public int CatalogId { get; set; }
        public string CatalogName { get; set; }
        public string Articul { get; set; }
        public int ModelId { get; set; }
        public int ColorId { get; set; }
        public string ColorName { get; set; }
        public string ColorValue { get; set; }
        public string ColorPhoto { get; set; }
        public int NomenclatureId { get; set; }
        public int PartsCount { get; set; }
        public int Amount { get; set; }
        public string Size { get; set; }
        public string SizeLine { get; set; }
        public string MainPhoto { get; set; }
        public string PhotoByColor { get; set; }
    }
}
