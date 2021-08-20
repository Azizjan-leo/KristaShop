using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Models;
using KristaShop.Common.Models.Filters;
using KristaShop.Common.Models.Session;
using Module.Common.Business.Models;
using Module.Partners.Business.DTOs;

namespace Module.Partners.Business.Interfaces {
    public interface IPartnerDocumentsService {
        Task<List<DocumentMovementAmountsDTO>> GetStorehouseDocumentsAsync(int userId, DocumentsFilter filter);
        Task<List<DocumentMovementAmountsDTO>> GetIncomeDocumentsAsync(int userId, DocumentsFilter filter);
        Task<List<DocumentMovementAmountsDTO>> GetSellingDocumentsAsync(int userId, DocumentsFilter filter);
        Task<List<DocumentMovementAmountsDTO>> GetRevisionDocumentsAsync(int userId, DocumentsFilter filter);
        Task<DocumentDTO<DocumentItemDetailedDTO>> GetDocumentAsync(ulong number, int userId);
        Task<DocumentDTO<ModelGroupedDTO>> GetDocumentWithGroupedItemsAsync(ulong number, int userId);
        Task<List<PaymentDocumentDTO<DocumentItemDetailedDTO>>> GetPaidPaymentDocumentsAsync(int userId);
        Task<List<PaymentDocumentDTO<DocumentItemDetailedDTO>>> GetPaidPaymentDocumentsByManagerAsync(UserSession session, PaymentsReportFilter filter);
        Task<List<PaymentDocumentDTO<DocumentItemDetailedDTO>>> GetNotPaidPaymentDocumentsAsync(int userId);
        Task UpdatePaymentDocumentStatusAsync(Guid documentId);
        IReadOnlyList<LookUpItem<string, string>> GetStorehouseDocumentsLookup();
        IReadOnlyList<LookUpItem<string, string>> GetPayableDocumentsLookup();
    }
}