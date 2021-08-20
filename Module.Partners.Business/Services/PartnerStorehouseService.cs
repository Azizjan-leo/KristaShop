using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KristaShop.Common.Enums;
using KristaShop.Common.Exceptions;
using KristaShop.Common.Extensions;
using KristaShop.Common.Implementation.ChainOfResponsibility;
using KristaShop.Common.Models.DTOs;
using KristaShop.Common.Models.Structs;
using Microsoft.EntityFrameworkCore;
using Module.Common.Business.Models;
using Module.Partners.Business.DTOs;
using Module.Partners.Business.Interfaces;
using Module.Partners.Business.Services.Operations;
using Module.Partners.Business.UnitOfWork;
using BarcodeAmountDTO = KristaShop.Common.Models.DTOs.BarcodeAmountDTO;

namespace Module.Partners.Business.Services {
    public class PartnerStorehouseService : IPartnerStorehouseService {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public PartnerStorehouseService(IUnitOfWork uow, IMapper mapper) {
            _uow = uow;
            _mapper = mapper;
        }
        
        public async Task<PartnerStorehouseItemDTO> GetStorehouseItemAsync(string barcode, int userId) {
            var item = await _uow.PartnerStorehouseItems.GetStorehouseItemAsync(barcode, userId);
            return (item != null) ? _mapper.Map<PartnerStorehouseItemDTO>(item) : null;
        }
        
        public async Task<List<PartnerStorehouseItemDTO>> GetStorehouseItemsAsync(int userId) {
            var items = await _uow.PartnerStorehouseItems.GetStorehouseItems(userId).ToListAsync();
            return _mapper.Map<List<PartnerStorehouseItemDTO>>(items);
        }
        
        public async Task<ItemsGroupedBase<ModelGroupedDTO, PaymentTotalInfo>> GetStorehouseItemsGroupedAsync(int userId) {
            var items = await _uow.PartnerStorehouseItems.GetStorehouseItems(userId).ToListAsync();
            var models = _mapper.Map<List<ModelGroupedDTO>>(items.GroupBy(x => x.GetModelKey()));
            
            return new ItemsGroupedBase<ModelGroupedDTO, PaymentTotalInfo> {
                Items = models,
                Totals = new PaymentTotalInfo(models.Sum(x => x.TotalAmount), models.Sum(x => x.TotalSum), await _uow.Partners.GetPartnerPaymentRateAsync(userId))
            };
        }

        public async Task<List<PartnerStorehouseItemDTO>> GetHistoryItems(int? userId, DateTimeOffset date = default, MovementDirection movementDirection = MovementDirection.None, MovementType movementType = MovementType.None, bool isAmountPositive = false) {
            var items = await _uow.PartnerStorehouseHistoryItems.GetGroupedHistoryItems(userId, date, movementDirection, movementType, isAmountPositive);
            return _mapper.Map<List<PartnerStorehouseItemDTO>>(items);
        }

        public async Task<List<BarcodeShipmentItemDTO>> GetShipmentsAsync(int userId) {
            var shipments = await _uow.Shipments.GetNotIncomedItemsWithBarcodes(userId);
            return _mapper.Map<List<BarcodeShipmentItemDTO>>(shipments);
        }
        
        public async Task<List<ItemsWithTotalsDTO<ModelGroupedDTO>>> GetShipmentsGroupedAsync(int userId) {
            var shipments = await _uow.Shipments.GetNotIncomedItemsWithBarcodes(userId);

            var result = shipments
                .GroupBy(
                    x => x.SaleDate.ToBasicString(),
                    (key, grouped) => {
                        var items = grouped.ToList();
                        return new ItemsWithTotalsDTO<ModelGroupedDTO> {
                            CreateDate = items.First().SaleDate,
                            Totals = new ReportTotalInfo(items.Sum(c => c.Amount), items.Sum(c => c.Price * c.Amount), items.Sum(c => c.PriceInRub * c.Amount)),
                            Items = _mapper.Map<List<ModelGroupedDTO>>(items.GroupBy(x => x.GetModelKey()))
                        };
                    })
                .OrderByDescending(o => o.CreateDate)
                .ToList();
            return result;
        }

        public async Task AutoIncomeShipmentItemsAsync(int userId, DateTime reservationDate) {
            var shipments = await _uow.Shipments.GetNotIncomedItemsWithBarcodes(userId, reservationDate);
            if (!shipments.Any()) {
                throw new ShipmentNotFoundException(reservationDate, userId);
            }
            
            var incomes = shipments.Select(x => new BarcodeAmountDTO(x.Barcodes.First(), x.Amount / x.Size.Parts)).ToList();
            await IncomeShipmentItemsAsync(new DTOs.BarcodeAmountDTO {
                UserId = userId,
                ReservationDate = reservationDate,
                Items = incomes
            });
        }

        public async Task IncomeShipmentItemsAsync(DTOs.BarcodeAmountDTO barcodeAmount) {
            await using (await _uow.BeginTransactionAsync()) {
                await ChainBuilder.CreateAsync(barcodeAmount, async builder => await builder
                    .NextAsync(new CreateIncomeListOperation(_uow))
                    .NextAsync(new CreateIncomeDocumentOperation(_uow, _mapper))
                    .NextAsync(new IncomeToStorehouseOperation(_uow))
                    .NextAsync(new AddToStorehouseHistoryOperation(_uow))
                ).ExecuteAsync();
                
                await _uow.SaveChangesAsync();
                await _uow.CommitTransactionAsync();
            }
        }

        public async Task SellStorehouseItemAsync(SellingDTO selling) {
            await using (await _uow.BeginTransactionAsync()) {
                await ChainBuilder.CreateAsync(selling, async builder => await builder
                    .NextAsync(new WriteOffSingleItemFromStorehouseOperation(_uow, _mapper))
                    .NextAsync(new CreateSellingDocumentOperation(_uow, _mapper))
                    .NextAsync(new AddToStorehouseHistoryOperation(_uow))
                ).ExecuteAsync();

                await _uow.SaveChangesAsync();
                await _uow.CommitTransactionAsync();
            }
        }

        public async Task AuditStorehouseItemsAsync(DTOs.BarcodeAmountDTO revision) {
            await using (await _uow.BeginTransactionAsync()) {
                await ChainBuilder.CreateAsync(revision, async builder => await builder
                    .NextAsync(new CreateRevisionListOperation(_uow, _mapper))
                    .NextAsync(new CreateRevisionDocumentOperation(_uow, _mapper))
                    .NextAsync(document => document.GetDeficiencyDocument(),
                        async innerBuilder => await innerBuilder
                            .NextAsync(new WriteOffFromStorehouseOperation(_uow))
                            .NextAsync(new AddToStorehouseHistoryOperation(_uow)),
                        ChainInputOption.SkipIfNull)
                    .NextAsync(document => document.GetExcessDocument(),
                        async innerBuilder => await innerBuilder
                            .NextAsync(new IncomeToStorehouseOperation(_uow))
                            .NextAsync(new AddToStorehouseHistoryOperation(_uow)),
                        ChainInputOption.SkipIfNull)
                ).ExecuteAsync();
                
                await _uow.SaveChangesAsync();
                await _uow.CommitTransactionAsync();
            }
        }
    }
}