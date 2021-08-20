using System.Collections.Generic;
using KristaShop.Common.Enums;
using KristaShop.DataAccess.Configurations.Partners;
using KristaShop.DataAccess.Entities.Interfaces.Partners;

namespace KristaShop.DataAccess.Entities.Partners {
    /// <summary>
    /// Configuration file for this entity <see cref="RevisionDeficiencyDocumentConfiguration"/>
    /// </summary>
    public class RevisionDeficiencyDocument : StorehouseDocument, IPayableDocument {
        public override string Name => "Недостача по ревизии";

        public RevisionDeficiencyDocument() {
            Direction = MovementDirection.Out;
        }

        public RevisionDeficiencyDocument(int userId, ICollection<DocumentItem> items) : base(userId, items) {
            Direction = MovementDirection.Out;
        }
    }
}