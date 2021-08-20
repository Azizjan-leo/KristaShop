using System.Collections.Generic;
using KristaShop.Common.Enums;
using Module.Catalogs.Business.Models;

namespace Module.Catalogs.WebUI.Models {
    public class SearchAllViewModel {
        public NomFilterDTO Filter { get; set; }
        public Dictionary<CatalogType, List<CatalogItemGroupNew>> SearchProducts { get; set; }
        public Dictionary<CatalogType, CatalogDTO> Catalogs { get; set; }
        public string SearchTitle { get; set; }
        public bool HasDescription { get; set; }
        public string DescriptionSettingKey { get; set; }
    }
}