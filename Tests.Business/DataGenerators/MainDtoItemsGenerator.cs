using KristaShop.Common.Enums;
using Module.Order.Business.Models;
using Tests.Common.DataGenerators;

namespace Tests.Business.DataGenerators {
    public class MainDtoItemsGenerator : DtoItemsGenerator {
        public CartItem1CDTO GenerateCartItem(int userId, int modelId, string forArticul, string size,
            int colorId, int amount, CatalogType catalogId, double price = 0) {

            return new() {
                UserId = userId,
                CatalogId = catalogId,
                ModelId = modelId,
                Articul = forArticul,
                NomenclatureId = 0,
                ColorId = colorId,
                SizeValue = size,
                Price = price,
                Amount = amount
            };
        }
    }
}