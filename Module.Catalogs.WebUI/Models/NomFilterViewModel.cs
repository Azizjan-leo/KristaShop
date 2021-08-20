using System.Collections.Generic;
using Module.Catalogs.Business.Models;
using P.Pager;

namespace Module.Catalogs.WebUI.Models
{
    public class NomFilterViewModel
    {
        public NomFilterDTO Filter { get; set; }
        public CatalogDTO Catalog { get; set; }
        public List<CatalogDTO> Catalogs { get; set; }
        public IPager<CatalogItemGroupNew> Models { get; set; }
        public int Page { get; set; }
    }
}