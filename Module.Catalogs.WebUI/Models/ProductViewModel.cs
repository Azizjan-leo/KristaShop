using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using KristaShop.Common.Enums;
using KristaShop.Common.Extensions;
using KristaShop.Common.Models;
using Module.Catalogs.Business.Models;
using Module.Common.Business.Models;

namespace Module.Catalogs.WebUI.Models {
    public class ProductViewModel {
        public CatalogItemGroupNew ItemFull { get; set; }
        [JsonIgnore]
        public CatalogDTO Catalog { get; set; }
        [JsonIgnore] 
        public CatalogItemDTO SelectedProduct { get; set; }
        [JsonIgnore]
        public bool IsFavorite { get; set; }

        public ProductViewModel(CatalogItemGroupNew itemFull, CatalogDTO catalog, bool excludePrices) {
            ItemFull = itemFull;
            Catalog = catalog;

            if (excludePrices) {
                ItemFull.CommonPrice = 0;
                ItemFull.CommonPriceInRub = 0;
            }


            var selectedSize = itemFull.CatalogItems.First(x => x.SizeValue.Equals(itemFull.Sizes.First()));
            SelectedProduct = selectedSize.Items.First(x => x.Color.Id == selectedSize.Colors.First().Id);
        }

        public string Serialize() {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }
    }
}
