using System;
using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Enums;
using KristaShop.Common.Implementation.ChainOfResponsibility.Operations;
using KristaShop.DataAccess.Entities.Partners;
using Module.Partners.Business.UnitOfWork;

namespace Module.Partners.Business.Services.Operations {
    public class IncomeToStorehouseOperation : ChainAsyncOperation<Document, Document> {
        private readonly IUnitOfWork _uow;

        public IncomeToStorehouseOperation(IUnitOfWork uow) {
            _uow = uow;
        }
        
        protected override async Task<Document> HandleInputAsync(Document document) {
            if (!document.Items.Any()) {
                return document;
            }
            
            var storehouseItems = document.Items
                .Select(x => new PartnerStorehouseItem {
                    Id = Guid.NewGuid(),
                    UserId = document.UserId,
                    Articul = x.Articul,
                    ModelId = x.ModelId,
                    ColorId = x.ColorId,
                    Size = x.Size,
                    Price = x.Price,
                    PriceInRub = x.PriceInRub,
                    Amount = x.Amount,
                    OrderType = PartnerOrderType.Basic,
                    IncomeDate = x.Date
                });

            await _uow.PartnerStorehouseItems.AddRangeAsync(storehouseItems);
            return document;
        }
    }
}