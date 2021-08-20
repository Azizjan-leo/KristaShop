using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using KristaShop.Common.Implementation.ChainOfResponsibility.Operations;
using KristaShop.DataAccess.Entities.Partners;
using Module.Partners.Business.DTOs;
using Module.Partners.Business.UnitOfWork;

namespace Module.Partners.Business.Services.Operations {
    public class CreateSellingDocumentOperation : ChainAsyncOperation<DocumentEditDTO, SellingDocument> {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CreateSellingDocumentOperation(IUnitOfWork uow, IMapper mapper) {
            _uow = uow;
            _mapper = mapper;
        }
        
        protected override async Task<SellingDocument> HandleInputAsync(DocumentEditDTO documentDto) {
            var createService = new DocumentsCreateService(_uow);
            var document = new SellingDocument(documentDto.UserId, _mapper.Map<List<DocumentItem>>(documentDto.Items));
            return await createService.CreateDocumentsAsync(document);
        }
    }
}