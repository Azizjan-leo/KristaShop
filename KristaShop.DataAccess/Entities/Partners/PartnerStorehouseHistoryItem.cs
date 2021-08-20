using System;
using KristaShop.Common.Enums;
using KristaShop.Common.Interfaces.Models;
using KristaShop.Common.Models.Structs;
using KristaShop.DataAccess.Configurations.Partners;

namespace KristaShop.DataAccess.Entities.Partners {
    /// <summary>
    /// Configuration file for this entity <see cref="PartnerStorehouseHistoryItemConfiguration"/>
    /// </summary>
    public class PartnerStorehouseHistoryItem : ICountableCatalogItem {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public MovementDirection Direction { get; set; }
        public MovementType MovementType { get; set; }
        public string Articul { get; set; }
        public int ModelId { get; set; }
        public SizeValue Size { get; set; }
        public int ColorId { get; set; }
        public double Price { get; set; }
        public double PriceInRub { get; set; }
        public int Amount { get; set; }
        public DateTimeOffset CreateDate { get; set; }

        public static PartnerStorehouseHistoryItem Income(ICatalogItemBase item, int amount, int userId) {
            return new() {
                Id = Guid.NewGuid(),
                UserId = userId,
                Direction = MovementDirection.In,
                MovementType = MovementType.Income,
                Articul = item.Articul,
                ModelId = item.ModelId,
                Size = item.Size,
                ColorId = item.ColorId,
                Price = item.Price,
                PriceInRub = item.PriceInRub,
                Amount = amount,
                CreateDate = DateTimeOffset.Now
            };
        }

        public static PartnerStorehouseHistoryItem Sell(ICatalogItemBase item, int amount, int userId) {
            return new() {
                Id = Guid.NewGuid(),
                UserId = userId,
                Direction = MovementDirection.Out,
                MovementType = MovementType.Selling,
                Articul = item.Articul,
                ModelId = item.ModelId,
                Size = item.Size,
                ColorId = item.ColorId,
                Price = -item.Price,
                PriceInRub = -item.PriceInRub,
                Amount = -amount,
                CreateDate = DateTimeOffset.Now
            };
        }

        public static PartnerStorehouseHistoryItem ExcessOrDeficiency(ICatalogItemBase item, MovementType movementType, int amount, int userId) {
            if (!(movementType == MovementType.IncomeAudit || movementType == MovementType.WriteOffAudit))
                throw new ArgumentException($"{nameof(movementType)} has to be either {MovementType.WriteOffAudit} or {MovementType.IncomeAudit}", nameof(movementType));

            return new PartnerStorehouseHistoryItem {
                Id = Guid.NewGuid(),
                UserId = userId,
                Direction = movementType == MovementType.WriteOffAudit ?  MovementDirection.Out : MovementDirection.In,
                MovementType = movementType,
                Articul = item.Articul,
                ModelId = item.ModelId,
                Size = item.Size,
                ColorId = item.ColorId,
                Price = (movementType == MovementType.WriteOffAudit) ? -item.Price : item.Price,
                PriceInRub = (movementType == MovementType.WriteOffAudit) ? -item.PriceInRub : item.PriceInRub,
                Amount = (movementType == MovementType.WriteOffAudit) ? -amount : amount,
                CreateDate = DateTimeOffset.Now
            };
        }

        public object Clone() {
            return this.MemberwiseClone();
        }
    }
}