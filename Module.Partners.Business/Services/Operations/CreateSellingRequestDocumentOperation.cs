using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KristaShop.Common.Exceptions;
using KristaShop.Common.Implementation.ChainOfResponsibility.Operations;
using KristaShop.DataAccess.Entities.Partners;
using Module.Partners.Business.DTOs;
using Module.Partners.Business.UnitOfWork;

namespace Module.Partners.Business.Services.Operations {
    public class CreateSellingRequestDocumentOperation  : ChainAsyncOperation<DocumentEditDTO, SellingRequestDocument>  {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CreateSellingRequestDocumentOperation(IUnitOfWork uow, IMapper mapper) {
            _uow = uow;
            _mapper = mapper;
        }
        
        protected override async Task<SellingRequestDocument> HandleInputAsync(DocumentEditDTO documentDto) {
            if (!documentDto.Items.Any()) {
                throw new DocumentItemsException(nameof(SellingRequestDocument));
            }
            
            var createService = new DocumentsCreateService(_uow);
            var document = new SellingRequestDocument(documentDto.UserId, _mapper.Map<List<DocumentItem>>(documentDto.Items));
            return await createService.CreateDocumentsAsync(document);
        }
    }
}