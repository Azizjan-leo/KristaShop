using System;
using System.Collections.Generic;
using System.Linq;
using KristaShop.Common.Enums;
using KristaShop.Common.Models.Structs;
using KristaShop.DataAccess.Configurations.DataFrom1C;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.DataFrom1C;
using KristaShop.DataAccess.Entities.Partners;

namespace Tests.Common.DataGenerators {
    public class EntityGenerator {
        public Model CreateModel(int id, string articul, string sizeValue, double price, int collectionId = 0) {
            var size = new SizeValue(sizeValue);
            return new() {
                Id = id,
                Name = $"{articul} ({size.Line})",
                Articul = articul,
                SizeLine = size.Line,
                Parts = size.Parts,
                Price = price,
                CollectionId = collectionId
            };
        }

        public Barcode CreateBarcode(int id, int modelId, int colorId, string sizeValue, string barcode) {
            return new() {
                Id = id,
                ModelId = modelId,
                ColorId = colorId,
                SizeValue = sizeValue,
                Value = barcode
            };
        }

        public List<Barcode> CreateBarcodesForLine(int startId, int modelId, int colorId, string sizeLine, int startBarcode) {
            var size = new SizeValue(sizeLine);
            List<Barcode> result = new();
            foreach (var sizeValue in size.Values) {
                result.Add(CreateBarcode(startId, modelId, colorId, sizeValue, $"{startBarcode:000}"));
                startId++;
                startBarcode++;
            }

            result.Add(CreateBarcode(startId, modelId, colorId, " ", $"{startBarcode:000}"));
            return result;
        }

        public Color CreateColor(int id, string name) {
            return new() {
                Id = id,
                Name = name
            };
        }

        public Color CreateColorWithGroup(int id, string name) {
            return new() {
                Id = id,
                Name = name,
                Group = new ColorGroup {
                    Id = id,
                    Name = name,
                    Hex = ""
                }
            };
        }

        public Partner CreatePartner(int userId, double paymentRate = 15) {
            return new() {
                UserId = userId,
                DateApproved = DateTimeOffset.Now.AddMonths(-1),
                PaymentRate = paymentRate
            };
        }

        public Shipment CreateShipment(int id, int userId, int modelId, int colorId, string sizeValue, int amount, double price) {
            return new() {
                Id = id,
                UserId = userId,
                ModelId = modelId,
                ColorId = colorId,
                SizeValue = sizeValue,
                Amount = amount,
                Price = price,
                PriceInRub = price * 80,
                ShipmentDate = DateTime.Today
            };
        }

        public CatalogItemDescriptor CreateDescriptor(string articul) {
            return new() {
                Articul = articul,
                Description = ""
            };
        }

        public ModelCatalogOrder CreateModelOrder(string articul, CatalogType catalogId, int position) {
            return new() {
                Articul = articul,
                CatalogId = catalogId,
                Order = position
            };
        }

        public Collection CreateCollection(int id, string name, int percent) {
            return new() {
                Id = id,
                Name = name,
                PercentValue = percent,
                Date = DateTime.Today,
                CreateDate = DateTime.Today
            };
        }
        
        public List<CartItem> GenerateCartItemsEntity(int userId, int count, int fromId, int modelId, string forArticul, string size,
            int colorId, int amount, CatalogType catalogId, double price = 0, DateTime addDate = default) {
            var result = new List<CartItem>();
            for (var i = 0; i < count; i++) {
                result.Add(new CartItem() {
                    Id = fromId,
                    UserId = userId,
                    CatalogId = catalogId,
                    Articul = forArticul,
                    ModelId = modelId,
                    SizeValue = size,
                    ColorId = colorId,
                    Amount = amount,
                    Price = price,
                    CreatedDate = addDate != default ? addDate : DateTime.Today 
                });

                fromId++;
            }

            return result;
        }

        public User CreateUsers(int id, int cityId, int managerId) {
            var user = new User {
                Id = id,
                Name = "",
                FullName = "",
                CityId = cityId,
                ManagerId = managerId
            };
            
            user.SetCatalogsAccesses(Enum.GetValues<CatalogType>().ToDictionary(k => k, v => true));
            return user;
        }

        public City CreateCity(int id, string name) {
            return new() {
                Id = id,
                Name = name
            };
        }
        
        public List<CatalogItem> GenerateCatalogItemsEntity(int count, int fromId, int modelId, string articul, string size,
            int colorId, int amount, CatalogType catalogId, double price = 0, DateTime addDate = default) {
            var result = new List<CatalogItem>();
            for (var i = 0; i < count; i++) {
                result.Add(new CatalogItem {
                    Id = fromId,
                    ModelId = modelId,
                    Articul = articul,
                    ColorId = colorId,
                    SizeValue = size,
                    NomenclatureId = string.IsNullOrEmpty(size) ? 0 : 1,
                    Amount = amount,
                    ExecutionDate = addDate != default ? addDate : DateTime.Today, 
                    CatalogId = catalogId,
                    Price = price,
                    PriceRub = price * 84
                });

                fromId++;
            }

            return result;
        }
    }
}