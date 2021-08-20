using System;
using System.Collections.Generic;
using KristaShop.Common.Models.Structs;

namespace KristaShop.Common.Interfaces.Models {
    public interface ICatalogItemBase : ICloneable {
        string Articul { get; set; }
        int ModelId { get; set; }
        SizeValue Size { get; set; }
        int ColorId { get; set; }
        double Price { get; set; }
        double PriceInRub { get; set; }
    }
    
    public interface ICountableCatalogItem : ICatalogItemBase {
        int Amount { get; set; }
    }

    public interface IBarcodesCatalogItem : ICatalogItemBase {
        List<string> Barcodes { get; set; }
    }

    public interface ISingleBarcodeCatalogItem : ICatalogItemBase {
        string Barcode { get; set; }
    }

    public interface IBarcodesCountableCatalogItem : ICountableCatalogItem, IBarcodesCatalogItem { }
}
