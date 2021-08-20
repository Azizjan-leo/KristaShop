using System.Collections.Generic;
using KristaShop.Common.Enums;
using KristaShop.Common.Models.Structs;
using KristaShop.DataAccess.Entities.Partners;

namespace Tests.Common.DataGenerators {
    public class DtoItemsGenerator {
        public const int RubRate = 82;
        public List<DocumentItem> CreateDocumentItems(int count, string forArticul, int fromSize) {
            var result = new List<DocumentItem>();
            for (var i = 0; i < count; i++) {
                result.Add(new DocumentItem {Articul = forArticul, Size = new SizeValue((fromSize + 2 * 0).ToString())});
            }

            return result;
        }
        
        public List<DocumentItem> CreateDocumentItems(int count, int modelId, string forArticul, int forSize, int colorId, int amount) {
            var result = new List<DocumentItem>();
            for (var i = 0; i < count; i++) {
                result.Add(new DocumentItem {
                    ModelId = modelId,
                    Articul = forArticul,
                    Size = new SizeValue(forSize.ToString()),
                    ColorId = colorId,
                    Amount = amount
                });
            }

            return result;
        }

        public IEnumerable<PartnerStorehouseItem> GenerateStorehouseItemsEntity(int userId, int count, int modelId,
            string forArticul, int forSize, int colorId, int amount) {
            var result = new List<PartnerStorehouseItem>();
            for (var i = 0; i < count; i++) {
                result.Add(new PartnerStorehouseItem {
                    UserId = userId,
                    ModelId = modelId,
                    Articul = forArticul,
                    Size = new SizeValue(forSize.ToString()),
                    ColorId = colorId,
                    Amount = amount
                });
            }

            return result;
        }
        
        public IEnumerable<PartnerStorehouseItem> GenerateStorehouseItemsEntity(int userId, int count, int modelId,
            string forArticul, int forSize, int colorId, int amount, double price) {
            var result = new List<PartnerStorehouseItem>();
            for (var i = 0; i < count; i++) {
                result.Add(new PartnerStorehouseItem {
                    UserId = userId,
                    ModelId = modelId,
                    Articul = forArticul,
                    Size = new SizeValue(forSize.ToString()),
                    ColorId = colorId,
                    Amount = amount,
                    Price = price,
                    PriceInRub = price * RubRate
                });
            }

            return result;
        }
        
        public IEnumerable<PartnerStorehouseItem> GenerateStorehouseItemsEntityFromLine(int userId, int count, int modelId,
            string forArticul, SizeValue size, int colorId, int amount) {
            var result = new List<PartnerStorehouseItem>();
            for (var i = 0; i < count; i++) {
                foreach (var sizeValue in size.Values) {
                    result.Add(new PartnerStorehouseItem {
                        UserId = userId,
                        ModelId = modelId,
                        Articul = forArticul,
                        Size = new SizeValue(sizeValue),
                        ColorId = colorId,
                        Amount = amount
                    });
                }
            }

            return result;
        }
    }
}