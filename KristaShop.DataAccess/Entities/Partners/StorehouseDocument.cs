using System.Collections.Generic;
using KristaShop.Common.Enums;

namespace KristaShop.DataAccess.Entities.Partners {
    /// <summary>
    /// Configuration file for this entity <see cref="StorehouseDocumentConfiguration"/>
    /// </summary>
    public class StorehouseDocument : Document {
        public override string Name => "Базовый складской документ";

        public StorehouseDocument() { }
        public StorehouseDocument(int userId, ICollection<DocumentItem> items) : base(userId, items, State.Completed) { }
        public StorehouseDocument(int userId, ICollection<DocumentItem> items, ICollection<Document> children) : base(userId, items, children, State.Completed) { }
    }
}