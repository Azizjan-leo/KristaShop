using System.Collections.Generic;
using KristaShop.Common.Enums;
using KristaShop.DataAccess.Configurations.Partners;
using KristaShop.DataAccess.Entities.Interfaces.Partners;

namespace KristaShop.DataAccess.Entities.Partners {
    /// <summary>
    /// Configuration file for this entity <see cref="SellingDocumentConfiguration"/>
    /// </summary>
    public class SellingDocument : StorehouseDocument, IPayableDocument {
        public override string Name => "Продажа";

        public SellingDocument() {
            Direction = MovementDirection.Out;
        }

        public SellingDocument(int userId, ICollection<DocumentItem> items) : base(userId, items) {
            Direction = MovementDirection.Out;
        }
    }
}