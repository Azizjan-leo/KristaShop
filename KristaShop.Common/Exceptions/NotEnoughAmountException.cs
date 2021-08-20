using KristaShop.Common.Enums;

namespace KristaShop.Common.Exceptions {
    public class NotEnoughAmountException : ExceptionBase {
        public NotEnoughAmountException(CatalogType catalogId, int modelId, int colorId, string sizeValue, int requestedAmount, int actualAmount)
            : base($"Not enough amount modelId: {modelId}, size: {sizeValue}, colorId: {colorId} in catalog: {catalogId}. Requested amount: {requestedAmount}, but actual {actualAmount}",
                $"Недостаточное количество товара в каталоге (в наличии {actualAmount}).") { }
        
        public NotEnoughAmountException(int userId, int modelId, int colorId, string sizeValue, int requestedAmount, int actualAmount)
            : base($"Not enough amount modelId: {modelId}, size: {sizeValue}, colorId: {colorId} in user: {userId} storehouse. Requested amount: {requestedAmount}, but actual {actualAmount}",
                $"Недостаточное количество товара на складе (в наличии {actualAmount}).") { }
    }
}