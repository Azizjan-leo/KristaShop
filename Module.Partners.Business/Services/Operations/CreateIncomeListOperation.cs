using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Exceptions;
using KristaShop.Common.Implementation.ChainOfResponsibility.Operations;
using KristaShop.Common.Interfaces.Models;
using KristaShop.Common.Models.Structs;
using Microsoft.EntityFrameworkCore;
using Module.Common.Business.Actions;
using Module.Partners.Business.DTOs;
using Module.Partners.Business.UnitOfWork;

namespace Module.Partners.Business.Services.Operations {
    public class CreateIncomeListOperation : ChainAsyncOperation<BarcodeAmountDTO, IncomeDocumentEditDTO> {
        private readonly IUnitOfWork _uow;

        public CreateIncomeListOperation(IUnitOfWork uow) {
            _uow = uow;
        }
        
        protected override async Task<IncomeDocumentEditDTO> HandleInputAsync(BarcodeAmountDTO barcodeAmount) {
            var shipments = await _uow.Shipments.GetNotIncomedItemsWithBarcodes(barcodeAmount.UserId, barcodeAmount.ReservationDate);
            if (!shipments.Any()) {
                throw new ShipmentNotFoundException(barcodeAmount.ReservationDate, barcodeAmount.UserId);
            }

            var incomeAmounts = barcodeAmount.Items
                .Where(x => x.Amount > 0)
                .GroupBy(x => x.Barcode)
                .ToDictionary(k => k.Key, v => v.Sum(x => x.Amount));

            var barcodeDetails = await _uow.Barcodes.GetBarcodes(incomeAmounts.Keys)
                .ToDictionaryAsync(k => k.Barcode, v => v);

            var shipmentsByBarcodes = shipments
                .SelectMany(x => x.Barcodes, (item, barcode) => new { item, barcode })
                .GroupBy(x => x.barcode)
                .ToDictionary(k => k.Key, v => v.First().item);

            var itemsToIncome = new List<DocumentItemDTO>();
            foreach (var barcode in incomeAmounts.Keys) {
                ICatalogItemBase item;
                if (shipmentsByBarcodes.ContainsKey(barcode)) {
                    item = shipmentsByBarcodes[barcode];
                } else if (barcodeDetails.ContainsKey(barcode)) {
                    item = barcodeDetails[barcode];
                } else {
                    continue;
                }

                itemsToIncome.AddRange(item.Size.Values.Select(sizeValue => new DocumentItemDTO {
                    Id = Guid.NewGuid(),
                    Articul = item.Articul,
                    ModelId = item.ModelId,
                    Size = new SizeValue(sizeValue),
                    ColorId = item.ColorId,
                    Amount = incomeAmounts[barcode],
                    Price = item.Price,
                    PriceInRub = item.PriceInRub
                }));
            }

            var (excesses, deficiencies) = CatalogItemsComparator.CheckForExcessAndDeficiency(itemsToIncome, shipments);

            var documentExcesses = excesses.Select(x => new DocumentItemDTO {
                Id = Guid.NewGuid(),
                Articul = x.Articul,
                ModelId = x.ModelId,
                Size = x.Size,
                ColorId = x.ColorId,
                Amount = x.Amount,
                Price = x.Price,
                PriceInRub = x.PriceInRub
            });
            
            var documentDeficiencies = deficiencies.Select(x => new DocumentItemDTO {
                Id = Guid.NewGuid(),
                Articul = x.Articul,
                ModelId = x.ModelId,
                Size = x.Size,
                ColorId = x.ColorId,
                Amount = x.Amount,
                Price = x.Price,
                PriceInRub = x.PriceInRub
            });

            return new IncomeDocumentEditDTO {
                Date = barcodeAmount.ReservationDate,
                UserId = barcodeAmount.UserId,
                Items = itemsToIncome,
                ExcessItems = documentExcesses,
                DeficiencyItems = documentDeficiencies
            };
        }
    }
}