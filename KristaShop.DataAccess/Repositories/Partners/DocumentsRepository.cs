#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Enums;
using KristaShop.Common.Exceptions;
using KristaShop.Common.Extensions;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.Common.Models;
using KristaShop.Common.Models.Filters;
using KristaShop.Common.Models.Structs;
using KristaShop.DataAccess.Entities.DataFrom1C;
using KristaShop.DataAccess.Entities.Partners;
using KristaShop.DataAccess.Interfaces.Repositories.Partners;
using Microsoft.EntityFrameworkCore;

namespace KristaShop.DataAccess.Repositories.Partners {
    public class DocumentsRepository : Repository<Document, Guid>, IDocumentsRepository {
        private static readonly IReadOnlyList<LookUpItem<string, string>> _storehouseDocumentsLookup;
        private static readonly IReadOnlyList<LookUpItem<string, string>> _payableDocumentsLookup;
        private static readonly List<string> _storehouseDocumentsTypes;
        private static readonly List<string> _storehouseMovementsDocumentsTypes;
        private static readonly List<string> _documentsWithGroupedItems;

        public DocumentsRepository(DbContext context) : base(context) { }

        static DocumentsRepository() {
            _storehouseDocumentsLookup = new List<Document> {new IncomeDocument(), new SellingDocument(), new RevisionDocument()}
                .Select(x => new LookUpItem<string, string>(x.GetType().Name, x.Name))
                .ToList();

            _payableDocumentsLookup = new List<Document> {new SellingDocument(), new RevisionDeficiencyDocument()}
                .Select(x => new LookUpItem<string, string>(x.GetType().Name, x.Name))
                .ToList();

            _storehouseDocumentsTypes = _storehouseDocumentsLookup.Select(x => x.Key).ToList();

            _storehouseMovementsDocumentsTypes = TypeReflectionExtensions.GetEnumerableOfType<StorehouseDocument>()
                .Where(x => x.Direction != MovementDirection.None)
                .Select(x => x.GetType().Name)
                .ToList();
            
            _documentsWithGroupedItems = new() {nameof(IncomeDocument), nameof(RevisionDocument)};
        }

        public async Task<IEnumerable<Document>> GetDocumentsAsync(int userId) {
            return await All.Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Number)
                .ToListAsync();
        }

        public async Task<IEnumerable<DocumentMovementAmounts>> GetStorehouseDocumentsMovementAmountsAsync(int userId, DocumentsFilter filter) {
            return await _getStorehouseDocumentsMovementAmountsAsync<StorehouseDocument>(userId, filter);
        }

        public async Task<IEnumerable<DocumentMovementAmounts>> GetIncomeDocumentsMovementAmountsAsync(int userId, DocumentsFilter filter) {
            return await _getStorehouseDocumentsMovementAmountsAsync<IncomeDocument>(userId, filter);
        }

        public async Task<IEnumerable<DocumentMovementAmounts>> GetSellingDocumentsMovementAmountsAsync(int userId, DocumentsFilter filter) {
            return await _getStorehouseDocumentsMovementAmountsAsync<SellingDocument>(userId, filter);
        }

        public async Task<IEnumerable<DocumentMovementAmounts>> GetRevisionDocumentsMovementAmountsAsync(int userId, DocumentsFilter filter) {
            return await _getStorehouseDocumentsMovementAmountsAsync<RevisionDocument>(userId, filter);
        }

        public async Task<IEnumerable<PaymentDocument>> GetPaidPaymentDocumentsWithItemsAsync(int userId) {
            return await Context.Set<PaymentDocument>().Where(x => x.UserId == userId)
                .Include(x => x.Items).ThenInclude(x => x.Model).ThenInclude(x => x.Descriptor)
                .Include(x => x.Items).ThenInclude(x => x.Color).ThenInclude(x => x.Group)
                .Include(x => x.Items).ThenInclude(x => x.FromDocument)
                .Where(x => x.State == State.Paid)
                .OrderByDescending(x => x.Number)
                .ToListAsync();
        }

        public async Task<IEnumerable<PaymentDocument>> GetPaidPaymentDocumentsWithItemsByManagerAsync(PaymentsReportFilter filter) {
            IQueryable<PaymentDocument> query = Context.Set<PaymentDocument>();

            if (filter.Cities.Any()) {
                query = query.Where(x => filter.Cities.Contains(x.Partner.User.CityId!.Value));
            }

            if (filter.Partners.Any()) {
                query = query.Where(x => filter.Partners.Contains(x.UserId));
            }
            
            if (filter.Managers.Any()) {
                query = query.Where(x => filter.Managers.Contains(x.Partner.User.ManagerId!.Value));
            }

            if (filter.Date.From.HasValue) {
                query = query.Where(x => x.CreateDate >= filter.Date.From);
            }

            if (filter.Date.To.HasValue) {
                query = query.Where(x => x.CreateDate <= filter.Date.To);
            }

            if (filter.DocumentTypes.Any()) {
                query = query.Where(x => x.Items.Any(c => filter.DocumentTypes.Contains(c.FromDocument!.DocumentType)));
            }
            
            return await query
                .Include(x => x.Items).ThenInclude(x => x.Model).ThenInclude(x => x.Descriptor)
                .Include(x => x.Items).ThenInclude(x => x.Color).ThenInclude(x => x.Group)
                .Include(x => x.Items).ThenInclude(x => x.FromDocument)
                .Include(x => x.Partner.User)
                .Where(x => x.State == State.Paid)
                .OrderByDescending(x => x.Number)
                .ToListAsync();
        }

        public async Task<IEnumerable<PaymentDocument>> GetNotPaidPaymentDocumentsWithItemsAsync(int userId) {
            return await Context.Set<PaymentDocument>()
                .Include(x => x.Items).ThenInclude(x => x.Model).ThenInclude(x => x.Descriptor)
                .Include(x => x.Items).ThenInclude(x => x.Color).ThenInclude(x => x.Group)
                .Include(x => x.Items).ThenInclude(x => x.FromDocument)
                .Where(x => x.UserId == userId && x.State != State.Paid)
                .OrderByDescending(x => x.Number)
                .ToListAsync();
        }

        public async Task<PaymentDocument?> GetLastNotPaidPaymentDocumentAsync(int userId) {
            return await Context.Set<PaymentDocument>()
                .Where(x => x.UserId == userId && x.State == State.NotPaid)
                .OrderByDescending(x => x.CreateDate)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<SellingRequestDocument>> GetSellingRequestsAsync(int userId) {
            return await Context.Set<SellingRequestDocument>()
                .Include(x => x.Items).ThenInclude(x => x.Model).ThenInclude(x => x.Descriptor)
                .Include(x => x.Items).ThenInclude(x => x.Color).ThenInclude(x => x.Group)
                .Include(x => x.Items).ThenInclude(x => x.FromDocument)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Number)
                .ToListAsync();
        }

        public async Task<SellingRequestDocument> GetNotApprovedSellingRequestAsync(int userId) {
            return await Context.Set<SellingRequestDocument>()
                .Include(x => x.Items).ThenInclude(x => x.Model).ThenInclude(x => x.Descriptor)
                .Include(x => x.Items).ThenInclude(x => x.Color).ThenInclude(x => x.Group)
                .Include(x => x.Items).ThenInclude(x => x.FromDocument)
                .Where(x => x.UserId == userId && x.State == State.Created)
                .OrderByDescending(x => x.Number)
                .FirstAsync();
        }

        public async Task<double> GetActivePaymentDocumentsTotalSumAsync(int userId) {
            try {

                var a = await Context.Set<SellingDocument>().ToListAsync();
                // return await Context.Set<PaymentDocument>()
                //     .Where(x => x.UserId == userId && x.State != State.Paid)
                //     .SumAsync(x => x.Sum);
                return 0;
            } catch (Exception ex) {
                Console.WriteLine(ex);
                throw;
            }
            
        }
        
        public async Task<Document> GetDocumentWithItemsAsync(ulong number, int userId) {
            var result = await All.Where(x => x.UserId == userId && x.Number == number)
                .Include(x => x.Items).ThenInclude(x => x.Model).ThenInclude(x => x.Descriptor)
                .Include(x => x.Items).ThenInclude(x => x.Color).ThenInclude(x => x.Group)
                .Include(x => x.Children).ThenInclude(x => x.Items)
                .FirstOrDefaultAsync();
            
            if (result == null) {
                throw new DocumentNotFoundException(number, userId);
            }

            return result;
        }
        
        public async Task<IEnumerable<DocumentItemView>> GetModelsMovementAsync(int userId, ModelsFilter filter) {
            var query = Context.Set<DocumentItem>()
                .Where(x => x.Document.UserId == userId && x.Document.Direction != MovementDirection.None);

            if (!string.IsNullOrEmpty(filter.Articul)) {
                query = query.Where(x => EF.Functions.Like(x.Articul, $"%{filter.Articul}%"));
            }
            
            if (!string.IsNullOrEmpty(filter.ColorName)) {
                query = query.Where(x => EF.Functions.Like(x.Color.Name, $"%{filter.ColorName}%"));
            }
            
            if (filter.Sizes.Any()) {
                var sizes = filter.Sizes.Select(c => new SizeValue(c)).ToList();
                query = query.Where(x => sizes.Contains(x.Size) || filter.Sizes.Contains(x.Model.SizeLine));
            }

            query = query.Where(x => _storehouseMovementsDocumentsTypes.Contains(x.Document.DocumentType));
            
            var result = await _groupModelsMovement(query.Where( x=> x.Document.CreateDate >= filter.DateFrom && x.Document.CreateDate <= filter.DateTo), TimePeriodType.AfterPeriodStarted)
                .Union(_groupModelsMovement(query.Where(x=>x.Document.CreateDate < filter.DateFrom), TimePeriodType.BeforePeriodStarted))
                .ToListAsync();
            return result;
        }

        private IQueryable<DocumentItemView> _groupModelsMovement(IQueryable<DocumentItem> query, TimePeriodType type) {
            return query
                .GroupBy(x => new {
                    Articul1 = x.Articul,
                    x.ModelId,
                    x.Size,
                    x.ColorId,
                    ColorName = x.Color.Name,
                    ColorHex = x.Color.Group.Hex,
                    x.Model.Name,
                    x.Model.SizeLine,
                    x.Model.Descriptor.MainPhoto,
                    x.Document.Direction
                })
                .Select(x => new DocumentItemView {
                    Articul = x.Key.Articul1,
                    ModelId = x.Key.ModelId,
                    MainPhoto = x.Key.MainPhoto,
                    Name = x.Key.Name,
                    SizeLine = x.Key.SizeLine,
                    Size = x.Key.Size,
                    ColorId = x.Key.ColorId,
                    Color = new Color {
                        Id = x.Key.ColorId,
                        Name = x.Key.ColorName,
                        Group = new ColorGroup {
                            Hex = x.Key.ColorHex
                        }
                    },
                    Amount = x.Sum(c => c.Amount),
                    Price = x.Average(c => c.Price),
                    PriceInRub = x.Average(c => c.PriceInRub),
                    Direction = x.Key.Direction,
                    Type = type
                });
        }
        
        public async Task<IEnumerable<DocumentItem>> GetModelMovementAsync(int userId, int modelId) {
            return await Context.Set<DocumentItem>()
                .Include(x => x.Document).ThenInclude(x => x.Parent)
                .Include(x => x.Model).ThenInclude(x => x.Descriptor)
                .Include(x => x.Color).ThenInclude(x => x.Group)
                .Where(x => x.Document.UserId == userId &&
                            x.ModelId == modelId &&
                            x.Document.Direction != MovementDirection.None &&
                            _storehouseMovementsDocumentsTypes.Contains(x.Document.DocumentType))
                .ToListAsync();
        }

        public IReadOnlyList<LookUpItem<string, string>> GetStorehouseDocumentsLookup() {
            return _storehouseDocumentsLookup;
        }

        public IReadOnlyList<LookUpItem<string, string>> GetPayableDocumentsLookup() {
            return _payableDocumentsLookup;
        }

        private async Task<IEnumerable<DocumentMovementAmounts>>
            _getStorehouseDocumentsMovementAmountsAsync<TDocument>(int userId, DocumentsFilter filter) where TDocument : StorehouseDocument {
            var query = Context.Set<TDocument>()
                .Where(x => x.UserId == userId);

            if (!string.IsNullOrEmpty(filter.Articul)) {
                query = query.Where(x => x.Items.Any(c => EF.Functions.Like(c.Articul, $"%{filter.Articul}%")));
            }
            
            if (!string.IsNullOrEmpty(filter.ColorName)) {
                query = query.Where(x => x.Items.Any(c => EF.Functions.Like(c.Color.Name, $"%{filter.ColorName}%")));
            }
            
            if (filter.Sizes.Any()) {
                var sizes = filter.Sizes.Select(c => new SizeValue(c)).ToList();
                query = query.Where(x => x.Items.Any(c=>sizes.Contains(c.Size) || filter.Sizes.Contains(c.Model.SizeLine)));
            }
            
            if (filter.DocumentTypes.Any()) {
                query = query.Where(x => filter.DocumentTypes.Contains(x.DocumentType));
            } else {
                query = query.Where(x => _storehouseDocumentsTypes.Contains(x.DocumentType));
            }
            
            query = query.Where(x => x.CreateDate >= filter.DateFrom && x.CreateDate <= filter.DateTo);
            
            return await query.OrderByDescending(x => x.Number)
                .Select(x => new DocumentMovementAmounts {
                    Id = x.Id,
                    Name = x.Name,
                    CreateDate = x.CreateDate,
                    Number = x.Number,
                    IncomeAmount = x.Direction == MovementDirection.Out
                        ? 0
                        : x.Direction == MovementDirection.In
                            ? x.Items.Sum(c => c.Amount)
                            : x.Children.Where(c => c.Direction == MovementDirection.In)
                                .Sum(c => c.Items.Sum(v => v.Amount)),
                    WriteOffAmount = x.Direction == MovementDirection.In
                        ? 0
                        : x.Direction == MovementDirection.Out
                            ? x.Items.Sum(c => c.Amount)
                            : x.Children.Where(c => c.Direction == MovementDirection.Out)
                                .Sum(c => c.Items.Sum(v => v.Amount)),
                    CanHaveGroupedItems = _documentsWithGroupedItems.Contains(x.DocumentType)
                })
                .ToListAsync();
        }
    }
}