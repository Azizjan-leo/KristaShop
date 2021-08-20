using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using KristaShop.Common.Implementation.ChainOfResponsibility.Operations;
using Module.Partners.Business.DTOs;
using Module.Partners.Business.UnitOfWork;

namespace Module.Partners.Business.Services.Operations {
    public class WriteOffSingleItemFromStorehouseOperation : ChainAsyncOperation<SellingDTO, DocumentEditDTO> {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public WriteOffSingleItemFromStorehouseOperation(IUnitOfWork uow, IMapper mapper) {
            _uow = uow;
            _mapper = mapper;
        }
        
        protected override async Task<DocumentEditDTO> HandleInputAsync(SellingDTO selling) {
            var item = await _uow.PartnerStorehouseItems.GetStorehouseItemAsync(selling.ModelId, selling.ColorId, selling.SizeValue, selling.UserId);
            
            if (item.Amount - 1 > 0) {
                item.Amount -= 1;
                _uow.PartnerStorehouseItems.Update(item);
            } else {
                _uow.PartnerStorehouseItems.Delete(item);
            }

            var documentItem = _mapper.Map<DocumentItemDTO>(item);
            documentItem.Amount = 1;
            
            return new DocumentEditDTO {
                UserId = selling.UserId,
                Items = new List<DocumentItemDTO> {
                    documentItem
                }
            };
        }
    }
}