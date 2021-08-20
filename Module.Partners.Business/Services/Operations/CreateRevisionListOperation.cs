using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KristaShop.Common.Exceptions;
using KristaShop.Common.Implementation.ChainOfResponsibility.Operations;
using Microsoft.EntityFrameworkCore;
using Module.Common.Business.Actions;
using Module.Partners.Business.DTOs;
using Module.Partners.Business.UnitOfWork;

namespace Module.Partners.Business.Services.Operations {
    public class CreateRevisionListOperation : ChainAsyncOperation<BarcodeAmountDTO, RevisionDocumentEditDTO> {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CreateRevisionListOperation(IUnitOfWork uow, IMapper mapper) {
            _uow = uow;
            _mapper = mapper;
        }

        protected override async Task<RevisionDocumentEditDTO> HandleInputAsync(BarcodeAmountDTO realStorehouseItems) {
            var actualStorehouseAmounts = realStorehouseItems.Items
                .GroupBy(x => x.Barcode)
                .ToDictionary(k => k.Key, v => v.Sum(x => x.Amount));

            var realStorehouseItemsBarcodes = await _uow.Barcodes.GetBarcodes(actualStorehouseAmounts.Keys).ToListAsync();
            if (realStorehouseItemsBarcodes.Any(x => x.Size.IsLine)) {
                throw new ExceptionBase("Revision input should not contain lines", "Ревизия не может быть проведена по линейкам");
            }
            
            var documentItems = realStorehouseItemsBarcodes.Select(item => new DocumentItemDTO {
                ModelId = item.ModelId,
                ColorId = item.ColorId,
                Price = item.Price,
                PriceInRub = item.PriceInRub,
                Size = item.Size,
                Amount = actualStorehouseAmounts[item.Barcode],
                Articul = item.Articul
            }).ToList();

            var dbStorehouseItems = await _uow.PartnerStorehouseItems.GetStorehouseItems(realStorehouseItems.UserId).ToListAsync();
            var (excess, deficiency) = CatalogItemsComparator.CheckForExcessAndDeficiency(documentItems, dbStorehouseItems);

            return new RevisionDocumentEditDTO {
                UserId = realStorehouseItems.UserId,
                Items = documentItems,
                DeficiencyItems = deficiency.Select(x => new DocumentItemDTO {
                    Id = Guid.NewGuid(),
                    Articul = x.Articul,
                    ModelId = x.ModelId,
                    Size = x.Size,
                    ColorId = x.ColorId,
                    Amount = x.Amount,
                    Price = x.Price,
                    PriceInRub = x.PriceInRub
                }),
                ExcessItems = excess.Select(x => new DocumentItemDTO {
                    Id = Guid.NewGuid(),
                    Articul = x.Articul,
                    ModelId = x.ModelId,
                    Size = x.Size,
                    ColorId = x.ColorId,
                    Amount = x.Amount,
                    Price = x.Price,
                    PriceInRub = x.PriceInRub
                })
            };
        }
    }
}