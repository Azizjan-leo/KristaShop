using KristaShop.DataAccess.Configurations.Partners;

namespace KristaShop.DataAccess.Entities.Partners {
    /// <summary>
    /// Configuration file for this entity <see cref="DocumentNumberSequenceConfiguration"/>
    /// </summary>
    public class DocumentNumberSequence {
        public ulong Id { get; set; }

        public DocumentNumberSequence(ulong id) {
            Id = id;
        }
    }
}