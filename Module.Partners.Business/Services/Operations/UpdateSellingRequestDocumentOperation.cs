using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KristaShop.Common.Implementation.ChainOfResponsibility.Operations;
using KristaShop.DataAccess.Entities.Partners;
using Module.Partners.Business.DTOs;
using Module.Partners.Business.UnitOfWork;

namespace Module.Partners.Business.Services.Operations {
    public class UpdateSellingRequestDocumentOperation :  ChainAsyncOperation<DocumentEditDTO, SellingRequestDocument> {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public UpdateSellingRequestDocumentOperation(IUnitOfWork uow, IMapper mapper) {
            _uow = uow;
            _mapper = mapper;
        }
        
        protected override async Task<SellingRequestDocument> HandleInputAsync(DocumentEditDTO updateDocument) {

            var request = await _uow.PartnerDocuments.GetNotApprovedSellingRequestAsync(updateDocument.UserId);

            var removedItems = request.Items
                .Where(x => !updateDocument.Items.Any(c => c.ModelId == x.ModelId && c.Size == x.Size && c.ColorId == x.ColorId))
                .ToList();
            
            foreach (var removedItem in removedItems) {
                request.Items.Remove(removedItem);
                _uow.PartnerDocumentItems.Delete(removedItem);
            }

            foreach (var updatedItem in updateDocument.Items) {
                var item = request.Items.FirstOrDefault(x => x.ModelId == updatedItem.ModelId && x.Size == updatedItem.Size && x.ColorId == updatedItem.ColorId);

                if (item == null) {
                    item = _mapper.Map<DocumentItem>(updatedItem);
                    item.Id = Guid.NewGuid();
                    item.Document = request;
                    request.Items.Add(item);
                    await _uow.PartnerDocumentItems.AddAsync(item);
                } else {
                    item.Amount = updatedItem.Amount;
                    _uow.PartnerDocumentItems.Update(item);
                }
            }

            return request;
        }
    }
}