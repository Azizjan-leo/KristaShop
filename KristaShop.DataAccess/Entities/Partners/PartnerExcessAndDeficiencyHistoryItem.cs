using System;
using KristaShop.Common.Enums;
using KristaShop.Common.Interfaces.Models;
using KristaShop.Common.Models.Structs;

namespace KristaShop.DataAccess.Entities.Partners {
    public class PartnerExcessAndDeficiencyHistoryItem : ICountableCatalogItem{
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public string Articul { get; set; }
        public int ModelId { get; set; }
        public SizeValue Size { get; set; }
        public int ColorId { get; set; }
        public double Price { get; set; }
        public double PriceInRub { get; set; }
        public int Amount { get; set; }
        public ExcessAndDeficiencyType Type { get; set; }
        public PartnerOperationResource Resource { get; set; }
        public DateTimeOffset CreateDate { get; set; }

        public static PartnerExcessAndDeficiencyHistoryItem CreateNew(int userId, IBarcodesCountableCatalogItem item, ExcessAndDeficiencyType type, PartnerOperationResource resource) {
            return new() {
                Id = new Guid(),
                UserId = userId,
                Articul = item.Articul,
                ModelId = item.ModelId,
                Size = item.Size,
                ColorId = item.ColorId,
                Price = item.Price,
                PriceInRub = 0,
                Amount = item.Amount,
                Type = type,
                Resource = resource,
                CreateDate = DateTimeOffset.Now
            };
        }

        public object Clone() {
            return this.MemberwiseClone();
        }
    }
}
