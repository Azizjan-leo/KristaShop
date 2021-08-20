using KristaShop.Common.Models.Structs;

namespace Module.Catalogs.Business.Models {
    public class ModelDescriptor1CDTO {
        private string _modelKey = string.Empty;
        private string _sizeKey = string.Empty;
        
        public int CatalogId { get; set; }
        public int ModelId { get; set; }
        public int NomenclatureId { get; set; }
        public string SizeLine { get; set; }
        public string Size { get; set; }
        public int ColorId { get; set; }

        public bool IsModelsLine => new SizeValue(Size, SizeLine).IsLine;

        public string SizeValue => new SizeValue(Size, SizeLine).IsLine ? SizeLine : Size;

        public string ProductPrefix => new SizeValue(Size, SizeLine).IsLine ? "L" : "S";

        public int ProductId => new SizeValue(Size, SizeLine).IsLine ? ModelId : NomenclatureId;

        public string ModelKey => string.IsNullOrEmpty(_modelKey)
            ? _modelKey = $"{ProductPrefix}_{CatalogId}_{ProductId}_{SizeValue}_{ColorId}"
            : _modelKey;

        public string SizeKey => string.IsNullOrEmpty(_sizeKey)
            ? _sizeKey = $"{ProductPrefix}_{CatalogId}_{ProductId}_{SizeValue}"
            : _sizeKey;
    }
}