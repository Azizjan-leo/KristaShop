using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using KristaShop.Common.Exceptions;
using KristaShop.Common.Implementation.ChainOfResponsibility;
using KristaShop.DataAccess.Entities.Partners;
using Module.Common.Business.Models;
using Module.Partners.Business.DTOs;
using Module.Partners.Business.Interfaces;
using Module.Partners.Business.Services.Operations;
using Module.Partners.Business.UnitOfWork;
using BarcodeAmountDTO = KristaShop.Common.Models.DTOs.BarcodeAmountDTO;

namespace Module.Partners.Business.Services {
    public class SellingRequestsService : ISellingRequestsService {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public SellingRequestsService(IUnitOfWork uow, IMapper mapper) {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<List<SellingRequestDocumentDTO<ModelGroupedDTO>>> GetRequestsAsync(int userId) {
            var requests = await _uow.PartnerDocuments.GetSellingRequestsAsync(userId);
            return _mapper.Map<List<SellingRequestDocumentDTO<ModelGroupedDTO>>>(requests);
        }

        public async Task CreateSellingRequestAsync(int userId, IEnumerable<BarcodeAmountDTO> items) {
            await using (await _uow.BeginTransactionAsync()) {
                await ChainBuilder.CreateAsync(new UserItems<BarcodeAmountDTO>(userId, items), async builder => await builder
                    .NextAsync(new GetSellingRequestItemsFromStorehouseOperation(_uow, _mapper))
                    .NextAsync(new CreateSellingRequestDocumentOperation(_uow, _mapper))
                ).ExecuteAsync();
                
                await _uow.SaveChangesAsync();
                await _uow.CommitTransactionAsync();
            }
        }

        public async Task UpdateSellingRequestAsync(int userId, IEnumerable<BarcodeAmountDTO> items) {
            await using (await _uow.BeginTransactionAsync()) {
                await ChainBuilder.CreateAsync(new UserItems<BarcodeAmountDTO>(userId, items), async builder => await builder
                    .NextAsync(new GetSellingRequestItemsFromStorehouseOperation(_uow, _mapper))
                    .NextAsync(new UpdateSellingRequestDocumentOperation(_uow, _mapper))
                ).ExecuteAsync();
                
                await _uow.SaveChangesAsync();
                await _uow.CommitTransactionAsync();
            }
        }

        public async Task UpdateSellingRequestStatusAsync(Guid documentId) {
            var document = await _uow.PartnerDocuments.GetByIdAsync(documentId);
            if (document is not SellingRequestDocument) {
                throw new DocumentNotFoundException(documentId);
            }
            
            document.SetNextState();
            await _uow.SaveChangesAsync();
        }
    }
}