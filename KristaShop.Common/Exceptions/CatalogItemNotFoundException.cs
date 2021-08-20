using KristaShop.Common.Enums;

namespace KristaShop.Common.Exceptions {
    public class CatalogItemNotFoundException : ExceptionBase {
          public CatalogItemNotFoundException(CatalogType catalogId, int modelId, int colorId, string sizeValue)
                            : base($"Item not found modelId: {modelId}, size: {sizeValue}, colorId: {colorId} not found in catalog: {catalogId}",
                                "Товар не найден в каталоге.") { }
    }
}