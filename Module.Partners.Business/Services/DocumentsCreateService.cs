using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Enums;
using KristaShop.Common.Exceptions;
using KristaShop.Common.Implementation.LockProvider;
using KristaShop.DataAccess.Entities.Interfaces.Partners;
using KristaShop.DataAccess.Entities.Partners;
using Module.Partners.Business.UnitOfWork;

namespace Module.Partners.Business.Services {
    public class DocumentsCreateService {
        private readonly IUnitOfWork _uow;
        private PaymentDocument _currentPaymentDocument;
        private static readonly LockProvider<int> PaymentsCreateLockProvider = new();

        public DocumentsCreateService(IUnitOfWork uow) {
            _uow = uow;
        }
        
        public async Task<Guid> CreateAndSaveDocumentsAsync(Document document) {
            if (document is IPayableDocument || document.Children.Any(x => x is IPayableDocument)) {
                _currentPaymentDocument = await _getOrCreatePaymentDocumentAsync(document.UserId);
            }

            await using (await _uow.BeginTransactionAsync()) {
                var result = await _createDocumentAsync(document);
                await _uow.SaveChangesAsync();
                await _uow.CommitTransactionAsync();
                return result;
            }
        }
        
        public async Task<T> CreateDocumentsAsync<T>(T document) where T : Document {
            if (document is IPayableDocument || document.Children.Any(x => x is IPayableDocument)) {
                _currentPaymentDocument = await _getOrCreatePaymentDocumentAsync(document.UserId);
            }
            
            await _createDocumentAsync(document);
            return document;
        }
        
        private async Task<Guid> _createDocumentAsync(Document document) {
            document.Number = await _uow.PartnerDocumentSequence.NextAsync();
            await _uow.PartnerDocuments.AddAsync(document, true);
            await _uow.PartnerDocumentItems.AddRangeAsync(document.Items);

            if (document is IPayableDocument && _currentPaymentDocument != null) {
                await _updateOrCreatePaymentAsync(document);
            }

            if (document.Children == null)
                return document.Id;
            
            foreach (var child in document.Children) {
                await _createDocumentAsync(child);
            }

            return document.Id;
        }
        
        private async Task _updateOrCreatePaymentAsync(Document document) {
            if (_currentPaymentDocument == null) throw new DocumentNotFoundException(typeof(PaymentDocument));

            var paymentRate = await _uow.Partners.GetPartnerPaymentRateAsync(document.UserId);
            await _uow.PartnerDocumentItems.AddRangeAsync(document.Items.Select(x => {
                var item = (DocumentItem) x.Clone();
                item.Id = Guid.NewGuid();
                item.Document = _currentPaymentDocument;
                item.FromDocument = document;
                item.Price = paymentRate;
                item.PriceInRub = 0;
                return item;
            }));
            _currentPaymentDocument.Sum += document.Items.Count * paymentRate;
        }
        
        private async Task<PaymentDocument> _getOrCreatePaymentDocumentAsync(int userId) {
            // lock payment create for current user only
            await PaymentsCreateLockProvider.WaitAsync(userId);
            try {
                var payment = await _uow.PartnerDocuments.GetLastNotPaidPaymentDocumentAsync(userId);
                if (payment == null) {
                    var paymentRate = await _uow.Partners.GetPartnerPaymentRateAsync(userId);
                    payment = new PaymentDocument(userId, paymentRate, new List<DocumentItem>(), State.NotPaid);
                    await _createDocumentAsync(payment);
                    await _uow.SaveChangesAsync();
                }
            
                return payment;
            } finally {
                PaymentsCreateLockProvider.Release(userId);
            }
        }
    }
}