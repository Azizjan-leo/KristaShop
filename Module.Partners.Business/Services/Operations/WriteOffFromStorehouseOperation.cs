using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Exceptions;
using KristaShop.Common.Implementation.ChainOfResponsibility.Operations;
using KristaShop.DataAccess.Entities.Partners;
using Microsoft.EntityFrameworkCore;
using Module.Partners.Business.UnitOfWork;

namespace Module.Partners.Business.Services.Operations {
    public class WriteOffFromStorehouseOperation : ChainAsyncOperation<Document, Document> {
        private readonly IUnitOfWork _uow;

        public WriteOffFromStorehouseOperation(IUnitOfWork uow) {
            _uow = uow;
        }
        
        protected override async Task<Document> HandleInputAsync(Document document) {
            if (!document.Items.Any()) {
                return document;
            }

            var itemsToWriteOff = document.Items
                .GroupBy(x => new {x.ModelId, x.ColorId, SizeValue = x.Size.Value})
                .Select(x => new {x.Key.ModelId, x.Key.ColorId, x.Key.SizeValue, Amount = x.Sum(c => c.Amount)});
            
            foreach (var deficiency in itemsToWriteOff) {
                var storehouseItemsPerBarcode = await _uow.PartnerStorehouseItems.All
                    .Where(x => x.UserId == document.UserId &&
                                x.ModelId == deficiency.ModelId &&
                                x.SizeValue == deficiency.SizeValue &&
                                x.ColorId == deficiency.ColorId)
                    .OrderByDescending(x => x.IncomeDate)
                    .ToListAsync();

                if (!storehouseItemsPerBarcode.Any()) {
                    throw new StorehouseItemNotFound(document.UserId, deficiency.ModelId, deficiency.ColorId, deficiency.SizeValue);
                }
                
                var totalAmountPerBarcode = storehouseItemsPerBarcode.Sum(x => x.Amount);
                var amountToWriteOff = deficiency.Amount;
                
                if (amountToWriteOff > totalAmountPerBarcode) {
                    throw new ExceptionBase("Too much to write off");
                }
                
                foreach (var item in storehouseItemsPerBarcode) {
                    item.Amount -= amountToWriteOff;
                    if (item.Amount < 1) {
                        amountToWriteOff = item.Amount * -1;
                        await _uow.PartnerStorehouseItems.DeleteAsync(item.Id);
                    }

                    if (item.Amount >= 0)
                        break;
                }
            }

            return document;
        }
    }
}