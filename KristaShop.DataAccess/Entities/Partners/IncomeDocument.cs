using System;
using System.Collections.Generic;
using KristaShop.Common.Enums;
using KristaShop.DataAccess.Configurations.Partners;

namespace KristaShop.DataAccess.Entities.Partners {
    /// <summary>
    /// Configuration file for this entity <see cref="IncomeDocumentConfiguration"/>
    /// </summary>
    public class IncomeDocument : StorehouseDocument {
        public DateTimeOffset ShipmentDate { get; set; }
        public override string Name => "Поступление";
        
        public IncomeDocument() {
            Direction = MovementDirection.In;
        }

        public IncomeDocument(int userId, ICollection<DocumentItem> items, DateTime shipmentDate) : base(userId, items) {
            ShipmentDate = DateTime.SpecifyKind(shipmentDate, DateTimeKind.Utc);
            Direction = MovementDirection.In;
        }
    }
}