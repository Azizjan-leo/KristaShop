using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.DataAccess.Interfaces.Repositories;
using KristaShop.DataAccess.Interfaces.Repositories.General;
using KristaShop.DataAccess.Interfaces.Repositories.Partners;

namespace Module.Partners.Business.UnitOfWork {
    public interface IUnitOfWork : IUnitOfWorkBase {
        IUserRepository Users { get; }
        IShipmentRepository Shipments { get; }
        IBarcodeRepository Barcodes { get; }
        IPartnerStorehouseItemRepository PartnerStorehouseItems { get; }
        IPartnerStorehouseHistoryRepository PartnerStorehouseHistoryItems { get; }
        IPartnerExcessAndDeficiencyRepository PartnerExcessAndDeficiencyItems { get; }
        IPartnershipRequestRepository PartnershipRequests { get; }
        IPartnersRepository Partners { get; }
        IDocumentsRepository PartnerDocuments { get; }
        IDocumentNumberSequenceRepository PartnerDocumentSequence { get; }
        IDocumentItemsRepository PartnerDocumentItems { get; }
    }
}