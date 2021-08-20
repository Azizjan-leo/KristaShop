namespace KristaShop.Common.Exceptions {
    public class StorehouseItemNotFound : ExceptionBase {
          public StorehouseItemNotFound(int userId, int modelId, int colorId, string sizeValue)
                    : base($"Storehouse item modelId: {modelId}, colorId: {colorId}, size: {sizeValue} not found in the user {userId} storehouse",
                        "Модель не найдена на складе партнера") { }
          
          public StorehouseItemNotFound(int userId, string barcode)
              : base($"Storehouse item {barcode} not found in the user {userId} storehouse",
                  "Модель не найдена на складе партнера") { }
    }
}