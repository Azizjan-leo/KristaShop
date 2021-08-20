using System.Collections.Generic;
using System.Linq;
using KristaShop.Common.Models.Structs;
using KristaShop.DataAccess.Configurations.Partners;

namespace KristaShop.DataAccess.Entities.Partners {
    /// <summary>
    /// Configuration file for this entity <see cref="RevisionDocumentConfiguration"/>
    /// </summary>
    public class RevisionDocument : StorehouseDocument {
        public override string Name => "Ревизия";
        
        public RevisionDocument() { }
        public RevisionDocument(int userId, ICollection<DocumentItem> items) : base(userId, items) { }
        public RevisionDocument(int userId, ICollection<DocumentItem> items, ICollection<Document> children) : base(userId, items, children) { }

        public RevisionDeficiencyDocument? GetDeficiencyDocument() {
            return (RevisionDeficiencyDocument) Children.FirstOrDefault(x => x.GetType() == typeof(RevisionDeficiencyDocument));
        }
        
        public RevisionExcessDocument? GetExcessDocument() {
            return (RevisionExcessDocument) Children.FirstOrDefault(x => x.GetType() == typeof(RevisionExcessDocument));
        }
    }
}