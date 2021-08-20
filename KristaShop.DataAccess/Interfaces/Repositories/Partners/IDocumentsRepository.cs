#nullable enable
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.Common.Models;
using KristaShop.Common.Models.Filters;
using KristaShop.DataAccess.Entities.Partners;

namespace KristaShop.DataAccess.Interfaces.Repositories.Partners {
    public interface IDocumentsRepository : IRepository<Document, Guid> {
        Task<IEnumerable<Document>> GetDocumentsAsync(int userId);
        Task<IEnumerable<DocumentMovementAmounts>> GetStorehouseDocumentsMovementAmountsAsync(int userId, DocumentsFilter filter);
        Task<IEnumerable<DocumentMovementAmounts>> GetIncomeDocumentsMovementAmountsAsync(int userId, DocumentsFilter filter);
        Task<IEnumerable<DocumentMovementAmounts>> GetSellingDocumentsMovementAmountsAsync(int userId, DocumentsFilter filter);
        Task<IEnumerable<DocumentMovementAmounts>> GetRevisionDocumentsMovementAmountsAsync(int userId, DocumentsFilter filter);
        Task<IEnumerable<PaymentDocument>> GetPaidPaymentDocumentsWithItemsAsync(int userId);
        Task<IEnumerable<PaymentDocument>> GetNotPaidPaymentDocumentsWithItemsAsync(int userId);
        Task<IEnumerable<PaymentDocument>> GetPaidPaymentDocumentsWithItemsByManagerAsync(PaymentsReportFilter filter);
        Task<PaymentDocument?> GetLastNotPaidPaymentDocumentAsync(int userId);
        Task<IEnumerable<SellingRequestDocument>> GetSellingRequestsAsync(int userId);
        Task<SellingRequestDocument> GetNotApprovedSellingRequestAsync(int userId);
        Task<Document> GetDocumentWithItemsAsync(ulong number, int userId);
        Task<double> GetActivePaymentDocumentsTotalSumAsync(int userId);
        Task<IEnumerable<DocumentItemView>> GetModelsMovementAsync(int userId, ModelsFilter filter);
        Task<IEnumerable<DocumentItem>> GetModelMovementAsync(int userId, int modelId);
        IReadOnlyList<LookUpItem<string, string>> GetStorehouseDocumentsLookup();
        IReadOnlyList<LookUpItem<string, string>> GetPayableDocumentsLookup();
    }
}