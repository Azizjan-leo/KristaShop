using System;
using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Enums;
using KristaShop.Common.Exceptions;
using KristaShop.Common.Implementation.ChainOfResponsibility.Operations;
using KristaShop.DataAccess.Entities.Partners;
using Module.Partners.Business.UnitOfWork;

namespace Module.Partners.Business.Services.Operations {
    public class AddToStorehouseHistoryOperation : ChainAsyncOperation<Document, Document> {
        private readonly IUnitOfWork _uow;

        public AddToStorehouseHistoryOperation(IUnitOfWork uow) {
            _uow = uow;
        }
        
        protected override async Task<Document> HandleInputAsync(Document document) {
            var movementType = document switch
            {
                IncomeDocument => MovementType.Income,
                SellingDocument => MovementType.Selling,
                RevisionDocument => MovementType.None,
                RevisionDeficiencyDocument => MovementType.WriteOffAudit,
                RevisionExcessDocument => MovementType.IncomeAudit,
                _ => MovementType.None
            };

            if (movementType == MovementType.None) {
                throw new ExceptionBase($"Movement type can not be {MovementType.None}");
            }
            
            var historyItems = document.Items.Select(x => new PartnerStorehouseHistoryItem {
                Id = Guid.NewGuid(),
                UserId = document.UserId,
                Direction = document.Direction,
                MovementType = movementType,
                Articul = x.Articul,
                ModelId = x.ModelId,
                Size = x.Size,
                ColorId = x.ColorId,
                Price = x.Price,
                PriceInRub = x.PriceInRub,
                Amount = x.Amount * (document.Direction == MovementDirection.In ? 1 : -1),
                CreateDate = DateTimeOffset.Now
            });
            
            await _uow.PartnerStorehouseHistoryItems.AddRangeAsync(historyItems);
            return document;
        }
    }
}