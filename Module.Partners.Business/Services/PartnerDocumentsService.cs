using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using KristaShop.Common.Exceptions;
using KristaShop.Common.Models;
using KristaShop.Common.Models.Filters;
using KristaShop.Common.Models.Session;
using KristaShop.DataAccess.Entities.Partners;
using Module.Common.Business.Models;
using Module.Partners.Business.DTOs;
using Module.Partners.Business.Interfaces;
using Module.Partners.Business.UnitOfWork;

namespace Module.Partners.Business.Services {
    public class PartnerDocumentsService : IPartnerDocumentsService {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public PartnerDocumentsService(IUnitOfWork uow, IMapper mapper) {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<List<DocumentMovementAmountsDTO>> GetStorehouseDocumentsAsync(int userId, DocumentsFilter filter) {
            var documents = await _uow.PartnerDocuments.GetStorehouseDocumentsMovementAmountsAsync(userId, filter);
            return _mapper.Map<List<DocumentMovementAmountsDTO>>(documents);
        }

        public async Task<List<DocumentMovementAmountsDTO>> GetIncomeDocumentsAsync(int userId, DocumentsFilter filter) {
            var documents = await _uow.PartnerDocuments.GetIncomeDocumentsMovementAmountsAsync(userId, filter);
            return _mapper.Map<List<DocumentMovementAmountsDTO>>(documents);
        }

        public async Task<List<DocumentMovementAmountsDTO>> GetSellingDocumentsAsync(int userId, DocumentsFilter filter) {
            var documents = await _uow.PartnerDocuments.GetSellingDocumentsMovementAmountsAsync(userId, filter);
            return _mapper.Map<List<DocumentMovementAmountsDTO>>(documents);
        }

        public async Task<List<DocumentMovementAmountsDTO>> GetRevisionDocumentsAsync(int userId, DocumentsFilter filter) {
            var documents = await _uow.PartnerDocuments.GetRevisionDocumentsMovementAmountsAsync(userId, filter);
            return _mapper.Map<List<DocumentMovementAmountsDTO>>(documents);    
        }

        public async Task<List<PaymentDocumentDTO<DocumentItemDetailedDTO>>> GetPaidPaymentDocumentsAsync(int userId) {
            var documents = await _uow.PartnerDocuments.GetPaidPaymentDocumentsWithItemsAsync(userId);
            return _mapper.Map<List<PaymentDocumentDTO<DocumentItemDetailedDTO>>>(documents);
        }

        public async Task<List<PaymentDocumentDTO<DocumentItemDetailedDTO>>> GetPaidPaymentDocumentsByManagerAsync(UserSession session, PaymentsReportFilter filter) {
            var documents = await _uow.PartnerDocuments.GetPaidPaymentDocumentsWithItemsByManagerAsync(filter);
            return _mapper.Map<List<PaymentDocumentDTO<DocumentItemDetailedDTO>>>(documents);
        }

        public async Task<List<PaymentDocumentDTO<DocumentItemDetailedDTO>>> GetNotPaidPaymentDocumentsAsync(int userId) {
            var result = await _uow.PartnerDocuments.GetNotPaidPaymentDocumentsWithItemsAsync(userId);
            return _mapper.Map<List<PaymentDocumentDTO<DocumentItemDetailedDTO>>>(result);
        }

        public async Task UpdatePaymentDocumentStatusAsync(Guid documentId) {
            var document = await _uow.PartnerDocuments.GetByIdAsync(documentId);
            if (document is not PaymentDocument payment) {
                throw new DocumentNotFoundException(documentId);
            }
            
            payment.SetNextState();
            await _uow.SaveChangesAsync();
        }
        
        public async Task<DocumentDTO<DocumentItemDetailedDTO>> GetDocumentAsync(ulong number, int userId) {
            var document = await _uow.PartnerDocuments.GetDocumentWithItemsAsync(number, userId);
            return _mapper.Map<DocumentDTO<DocumentItemDetailedDTO>>(document);
        }
        
        public async Task<DocumentDTO<ModelGroupedDTO>> GetDocumentWithGroupedItemsAsync(ulong number, int userId) {
            var document = await _uow.PartnerDocuments.GetDocumentWithItemsAsync(number, userId);
            return _mapper.Map<DocumentDTO<ModelGroupedDTO>>(document);
        }

        public IReadOnlyList<LookUpItem<string, string>> GetStorehouseDocumentsLookup() {
            return _uow.PartnerDocuments.GetStorehouseDocumentsLookup();
        }
        
        public IReadOnlyList<LookUpItem<string, string>> GetPayableDocumentsLookup() {
            return _uow.PartnerDocuments.GetPayableDocumentsLookup();
        }
    }
}