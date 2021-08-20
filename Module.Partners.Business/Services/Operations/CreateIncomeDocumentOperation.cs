using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using KristaShop.Common.Implementation.ChainOfResponsibility.Operations;
using KristaShop.DataAccess.Entities.Partners;
using Module.Partners.Business.DTOs;
using Module.Partners.Business.UnitOfWork;

namespace Module.Partners.Business.Services.Operations {
    public class CreateIncomeDocumentOperation : ChainAsyncOperation<IncomeDocumentEditDTO, IncomeDocument> {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CreateIncomeDocumentOperation(IUnitOfWork uow, IMapper mapper) {
            _uow = uow;
            _mapper = mapper;
        }
        
        protected override async Task<IncomeDocument> HandleInputAsync(IncomeDocumentEditDTO documentDto) {
            var createService = new DocumentsCreateService(_uow);
            var document = new IncomeDocument(documentDto.UserId, _mapper.Map<List<DocumentItem>>(documentDto.Items), documentDto.Date);
            return await createService.CreateDocumentsAsync(document);
        }
    }
}