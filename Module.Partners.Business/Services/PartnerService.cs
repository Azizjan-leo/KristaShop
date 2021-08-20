using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KristaShop.Common.Exceptions;
using KristaShop.Common.Models;
using KristaShop.Common.Models.DTOs;
using KristaShop.Common.Models.Filters;
using KristaShop.Common.Models.Structs;
using KristaShop.DataAccess.Entities.Partners;
using Microsoft.EntityFrameworkCore;
using Module.Common.Business.Interfaces;
using Module.Common.Business.Models;
using Module.Partners.Business.DTOs;
using Module.Partners.Business.Interfaces;
using Module.Partners.Business.UnitOfWork;

namespace Module.Partners.Business.Services {
    public class PartnerService : IPartnerService {
        private readonly IUnitOfWork _uow;
        private readonly ISettingsManager _settingsManager;
        private readonly IMapper _mapper;

        public PartnerService(IUnitOfWork uow, ISettingsManager settingsManager, IMapper mapper) {
            _uow = uow;
            _settingsManager = settingsManager;
            _mapper = mapper;
        }

        public async Task<OperationResult> AcceptRequestToProcessAsync(Guid id) {
            var userRequest = await _uow.PartnershipRequests.GetByIdAsync(id);
            if (userRequest == null)
                throw new KeyNotFoundException("The partner request not found!");
            userRequest.IsAcceptedToProcess = !userRequest.IsAcceptedToProcess;
            var result = await _uow.SaveChangesAsync();

            return result > 0 ? OperationResult.Success("Операция выполнена успешно!") : OperationResult.Failure("Произошла ошибка!");
        }

        public async Task<OperationResult> ApplyAsync(int userId) {
            var userRequest = await _uow.PartnershipRequests.All.Where(x => x.UserId == userId).FirstOrDefaultAsync();
            if(userRequest != null)
                return OperationResult.Success(_settingsManager.Settings.PartnershipRequstActiveRequest);

            await _uow.PartnershipRequests.AddAsync(new PartnershipRequest(userId, DateTime.UtcNow), true);
            await _uow.SaveChangesAsync();
            return OperationResult.Success(_settingsManager.Settings.PartnershipRequestAcceptedToProcess);
        }

        public async Task<OperationResult> ApproveRequestAsync(Guid id) {
            var request = await _uow.PartnershipRequests.GetByIdAsync(id);
            if (request == null)
                throw new KeyNotFoundException("The partner request not found!");
            var user = await _uow.Users.GetByIdAsync(request.UserId);
            if(user?.Data is null)
                throw new KeyNotFoundException("User data not found!");

            await using (await _uow.BeginTransactionAsync()) {
                await _uow.Partners.AddAsync(new Partner(user.Id, _settingsManager.Settings.DefaultPartnerPaymentRate));

                request.IsConfirmed = true;
                request.AnsweredDate = DateTime.UtcNow;

                _uow.PartnershipRequests.Update(request);
                await _uow.SaveChangesAsync();
                await _uow.CommitTransactionAsync();
            }

            return OperationResult.Success("Запрос удовлетворен!");
        }

        public async Task<OperationResult> DeleteRequestAsync(Guid id) {
            var userRequest = await _uow.PartnershipRequests.DeleteAsync(id);
            if (userRequest == null)
                throw new KeyNotFoundException("The partner request not found!");
          
            var result = await _uow.SaveChangesAsync();

            return result > 0 ? OperationResult.Success("Запрос удален!") : OperationResult.Failure("Произошла ошибка!");
        }

        public async Task<ItemsGroupedBase<PartnerDTO, PartnersTotalsDTO>> GetAllPartnersAsync(PartnersFilter filter) {
            var partners = await _uow.Partners.GetAllPartnersAsync(filter);

            var partnerSqlViews = partners.ToList();
            var result = new ItemsGroupedBase<PartnerDTO, PartnersTotalsDTO> {
                Items = _mapper.Map<List<PartnerDTO>>(partners),
                Totals = new PartnersTotalsDTO {
                    TotalAmount = partnerSqlViews.Sum(x => x.StorehouseItemsCount),
                    TotalShippedAmount = partnerSqlViews.Sum(x => x.ShipmentsItemsCount),
                    TotalDebt = partnerSqlViews.Sum(x => x.DebtItemsSum)
                }
            };
            return result;
        }

        public async Task<List<PartnershipRequestDTO>> GetNewRequests() {
            var newRequests = await _uow.PartnershipRequests.GetRequests();
            return _mapper.Map<List<PartnershipRequestDTO>>(newRequests);
        }

        public async Task<List<LookUpItem<int, string>>> GetPartnersLookUpAsync() => await _uow.Partners.GetPartnersLookUpAsync();

        public async Task<ItemsGrouped<PartnerSalesReportItem>> GetSalesReportAsync(ReportsFilter filter) {
            var list = await _uow.Partners.GetSalesReportItems(filter);
            return new ItemsGrouped<PartnerSalesReportItem> {
                Items = list,
                Totals = new ReportTotalInfo(list.Sum(x => x.Amount), list.Sum(x => x.Sum), 0)
            };
        }
        
        public async Task<List<SimpleGroupedModelDTO<DocumentItemDetailedDTO>>> GetDecryptedSalesReportAsync(ReportsFilter filter) {
            var source = await _uow.Partners.GetDecryptedSalesReportItems(filter);
            var list = source.GroupBy(x => x.ModelId);
            var result = new List<SimpleGroupedModelDTO<DocumentItemDetailedDTO>>();

            foreach (var item in list) {
                var first = item.First();
                result.Add(new SimpleGroupedModelDTO<DocumentItemDetailedDTO> {
                    ModelKey = item.Key.ToString(),
                    ModelInfo = new ModelInfoDTO() {
                        Name = first.Model.Name,
                        Articul = first.Model.Articul,
                        MainPhoto = first.Model.Descriptor.MainPhoto,
                        Size = first.Size
                    },
                    Date = first.Date,
                    TotalAmount = item.Sum(x => x.Amount),
                    TotalSum = item.Sum(x => x.Price),
                    Items = _mapper.Map<List<DocumentItemDetailedDTO>>(item.GroupBy(x => new {
                        x.ModelId,
                        x.ColorId,
                        x.Size
                    })
                    .Select(x=> {
                        var first =  x.First();
                        first.Amount = x.Sum(c => c.Amount);
                        first.Price = x.Sum(c => c.Price);
                        return first;
                    })
                    .ToList()).OrderBy(x => x.ColorName).ThenBy(x => x.Size.Value).ToList()
                });
            }

            return result;
        }

        public async Task MakePartnerAsync(int userId) {
            var user = await _uow.Users.GetByIdAsync(userId);
            if (user == null) {
                throw new EntityNotFoundException($"User not found {userId}");
            }
            
            var partner = new Partner(userId, _settingsManager.Settings.DefaultPartnerPaymentRate);
            await _uow.Partners.AddAsync(partner);
            await _uow.SaveChangesAsync();
        }
    }
}