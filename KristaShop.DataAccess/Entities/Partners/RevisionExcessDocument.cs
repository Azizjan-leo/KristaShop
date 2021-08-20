using System.Collections.Generic;
using KristaShop.Common.Enums;
using KristaShop.DataAccess.Configurations.Partners;

namespace KristaShop.DataAccess.Entities.Partners {
    /// <summary>
    /// Configuration file for this entity <see cref="RevisionExcessDocumentConfiguration"/>
    /// </summary>
    public class RevisionExcessDocument : StorehouseDocument {
        public override string Name => "Избыток по ревизии";

        public RevisionExcessDocument() {
            Direction = MovementDirection.In;
        }

        public RevisionExcessDocument(int userId, ICollection<DocumentItem> items) : base(userId, items) {
            Direction = MovementDirection.In;
        }
    }
}