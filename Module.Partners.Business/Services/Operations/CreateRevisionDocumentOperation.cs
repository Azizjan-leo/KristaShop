using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KristaShop.Common.Implementation.ChainOfResponsibility.Operations;
using KristaShop.DataAccess.Entities.Partners;
using Module.Partners.Business.DTOs;
using Module.Partners.Business.UnitOfWork;

namespace Module.Partners.Business.Services.Operations {
    public class CreateRevisionDocumentOperation : ChainAsyncOperation<RevisionDocumentEditDTO, RevisionDocument> {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CreateRevisionDocumentOperation(IUnitOfWork uow, IMapper mapper) {
            _uow = uow;
            _mapper = mapper;
        }
        
        protected override async Task<RevisionDocument> HandleInputAsync(RevisionDocumentEditDTO documentEditDTO) {
            var createService = new DocumentsCreateService(_uow);
            var document = new RevisionDocument(documentEditDTO.UserId,
                _mapper.Map<List<DocumentItem>>(documentEditDTO.Items),
                _createRevisionExcessAndDeficiencyDocuments(documentEditDTO));
            
            return await createService.CreateDocumentsAsync(document);
        }

        private ICollection<Document> _createRevisionExcessAndDeficiencyDocuments(RevisionDocumentEditDTO documentEditDTO) {
            var result = new List<Document>();
            if (documentEditDTO.ExcessItems.Any()) {
                result.Add(new RevisionExcessDocument(documentEditDTO.UserId,
                    _mapper.Map<List<DocumentItem>>(documentEditDTO.ExcessItems)));
            }

            if (documentEditDTO.DeficiencyItems.Any()) {
                result.Add(new RevisionDeficiencyDocument(documentEditDTO.UserId,
                    _mapper.Map<List<DocumentItem>>(documentEditDTO.DeficiencyItems)));
            }

            return result;
        }
    }
}