using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KristaShop.Common.Exceptions;
using KristaShop.Common.Implementation.ChainOfResponsibility.Operations;
using Microsoft.EntityFrameworkCore;
using Module.Common.Business.Models;
using Module.Partners.Business.DTOs;
using Module.Partners.Business.UnitOfWork;
using BarcodeAmountDTO = KristaShop.Common.Models.DTOs.BarcodeAmountDTO;

namespace Module.Partners.Business.Services.Operations {
    public class GetSellingRequestItemsFromStorehouseOperation : ChainAsyncOperation<UserItems<BarcodeAmountDTO>, DocumentEditDTO> {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetSellingRequestItemsFromStorehouseOperation(IUnitOfWork uow, IMapper mapper) {
            _uow = uow;
            _mapper = mapper;
        }
        
        protected override async Task<DocumentEditDTO> HandleInputAsync(UserItems<BarcodeAmountDTO> data) {
            if (!data.Items.Any()) {
                throw new EmptyListException();
            }

            var requestedItems = data.Items
                .Where(x => x.Amount > 0)
                .GroupBy(x => x.Barcode)
                .Select(x => new BarcodeAmountDTO(x.Key, x.Sum(c => c.Amount)));
            
            var storehouseItems = await _uow.PartnerStorehouseItems.GetStorehouseItems(data.UserId).ToListAsync();
            
            var result = new List<DocumentItemDTO>();
            foreach (var item in requestedItems) {
                var storehouseItem = storehouseItems.FirstOrDefault(x => x.Barcodes.Contains(item.Barcode));
                
                if (storehouseItem == null) {
                    throw new StorehouseItemNotFound(data.UserId, item.Barcode);
                }

                if (storehouseItem.Amount < item.Amount) {
                    throw new NotEnoughAmountException(data.UserId, storehouseItem.ModelId, storehouseItem.ColorId,
                        storehouseItem.Size.Value, item.Amount, storehouseItem.Amount);
                }

                var documentItem = _mapper.Map<DocumentItemDTO>(storehouseItem);
                documentItem.Amount = item.Amount;
                result.Add(documentItem);
            }

            return new DocumentEditDTO {
                UserId = data.UserId,
                Items = _mapper.Map<List<DocumentItemDTO>>(result)
            };
        }
    }
}