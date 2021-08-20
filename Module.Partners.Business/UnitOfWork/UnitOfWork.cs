using System;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.Common.Interfaces;
using KristaShop.Common.Models.Session;
using KristaShop.DataAccess.CacheRepositories.General;
using KristaShop.DataAccess.CacheRepositories.Partners;
using KristaShop.DataAccess.Domain;
using KristaShop.DataAccess.Interfaces.Repositories;
using KristaShop.DataAccess.Interfaces.Repositories.General;
using KristaShop.DataAccess.Interfaces.Repositories.Partners;
using KristaShop.DataAccess.Repositories;
using KristaShop.DataAccess.Repositories.General;
using KristaShop.DataAccess.Repositories.Partners;
using Microsoft.Extensions.Caching.Memory;
using Serilog;

namespace Module.Partners.Business.UnitOfWork {
    public class UnitOfWork : UnitOfWorkBase, IUnitOfWork {
        private readonly Lazy<IUserRepository> _usersRepository;
        private readonly Lazy<IShipmentRepository> _shipmentsRepository;
        private readonly Lazy<IBarcodeRepository> _barcodeRepository;
        private readonly Lazy<IPartnerStorehouseItemRepository> _partnerStorehouseItemRepository;
        private readonly Lazy<IPartnerStorehouseHistoryRepository> _partnerStorehouseHistoryItemsRepository;
        private readonly Lazy<IPartnerExcessAndDeficiencyRepository> _partnerExcessAndDeficiencyRepository;
        private readonly Lazy<IPartnershipRequestRepository> _partnershipRequestRepository;
        private readonly Lazy<IPartnersRepository> _partnerRepository;
        private readonly Lazy<IDocumentsRepository> _partnerDocumentsRepository;
        private readonly Lazy<IDocumentNumberSequenceRepository> _partnerDocumentSequenceRepository;
        private readonly Lazy<IDocumentItemsRepository> _partnerDocumentItemsRepository;

        public IUserRepository Users => _usersRepository.Value;
        public IShipmentRepository Shipments => _shipmentsRepository.Value;
        public IBarcodeRepository Barcodes => _barcodeRepository.Value;
        public IPartnerStorehouseItemRepository PartnerStorehouseItems => _partnerStorehouseItemRepository.Value;
        public IPartnerStorehouseHistoryRepository PartnerStorehouseHistoryItems => _partnerStorehouseHistoryItemsRepository.Value;
        public IPartnerExcessAndDeficiencyRepository PartnerExcessAndDeficiencyItems => _partnerExcessAndDeficiencyRepository.Value;
        public IPartnershipRequestRepository PartnershipRequests => _partnershipRequestRepository.Value;
        public IPartnersRepository Partners  => _partnerRepository.Value;
        public IDocumentsRepository PartnerDocuments => _partnerDocumentsRepository.Value;
        public IDocumentNumberSequenceRepository PartnerDocumentSequence => _partnerDocumentSequenceRepository.Value;
        public IDocumentItemsRepository PartnerDocumentItems => _partnerDocumentItemsRepository.Value;
        
        public UnitOfWork(KristaShopDbContext context, IMemoryCache memoryCache, IClaimsManager claimsManager, ILogger logger) : base(context, logger, claimsManager) {
            _usersRepository = new Lazy<IUserRepository>(() => new UserCacheRepository(memoryCache, new UserRepository(context)));
            _shipmentsRepository = new Lazy<IShipmentRepository>(() => new ShipmentRepository(context));
            _barcodeRepository = new Lazy<IBarcodeRepository>(() => new BarcodeRepository(context));
            _partnerStorehouseItemRepository = new Lazy<IPartnerStorehouseItemRepository>(() => new PartnerStorehouseItemRepository(Context));
            _partnerStorehouseHistoryItemsRepository = new Lazy<IPartnerStorehouseHistoryRepository>(() => new PartnerStorehouseHistoryRepository(Context));
            _partnerExcessAndDeficiencyRepository = new Lazy<IPartnerExcessAndDeficiencyRepository>(() => new PartnerExcessAndDeficiencyRepository(Context));
            _partnershipRequestRepository = new Lazy<IPartnershipRequestRepository>(() => new PartnershipRequestRepository(Context));
            _partnerRepository = new Lazy<IPartnersRepository>(() => new PartnersCacheRepository(memoryCache, new PartnersRepository(Context)));
            _partnerDocumentsRepository = new Lazy<IDocumentsRepository>(() => new DocumentsRepository(Context));
            _partnerDocumentSequenceRepository = new Lazy<IDocumentNumberSequenceRepository>(() => new DocumentNumberSequenceRepository(Context));
            _partnerDocumentItemsRepository = new Lazy<IDocumentItemsRepository>(() => new DocumentItemsRepository(Context));
        }
    }
}