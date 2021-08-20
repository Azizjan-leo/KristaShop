using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Enums;
using KristaShop.Common.Extensions;
using KristaShop.Common.Models.Filters;
using KristaShop.Common.Models.Structs;
using Microsoft.EntityFrameworkCore;
using Module.Common.Business.Models;
using Module.Partners.Business.DTOs;
using Module.Partners.Business.Interfaces;
using Module.Partners.Business.UnitOfWork;

namespace Module.Partners.Business.Services {
    public class PartnerStorehouseReportService : IPartnerStorehouseReportService {
        private readonly IUnitOfWork _uow;

        public PartnerStorehouseReportService(IUnitOfWork uow) {
            _uow = uow;
        }

        public async Task<PartnerTotalsDTO> GetTotalsAsync(int userId) {
            var result = await _uow.PartnerStorehouseItems.All
                .Where(x => x.UserId == userId)
                .GroupBy(x => new {x.Articul, x.ColorId, x.SizeValue})
                .Select(x => new {
                    TotalAmount = x.Sum(y => y.Amount),
                    TotalQuantity = x.Count()
                })
                .GroupBy(x => true)
                .Select(x => new {
                    TotalAmount = x.Sum(c => c.TotalAmount),
                    TotalQuantity = x.Count()
                })
                .FirstOrDefaultAsync();

            return new PartnerTotalsDTO {
                Amount = result.TotalAmount,
                ModelsQuantity = result.TotalQuantity,
                TotalToPay = await _uow.PartnerDocuments.GetActivePaymentDocumentsTotalSumAsync(userId)
            };
        }

        public async Task<List<StorehouseMovementGroupDTO<StorehouseMovementItemDTO>>> GetModelsMovementAsync(int userId, ModelsFilter filter) {
            var items = await _uow.PartnerDocuments.GetModelsMovementAsync(userId, filter);
        
            var result = items.GroupBy(x => x.GetModelKey(), (key, groupedItems) => {
                var groupedItemsList = groupedItems.ToList();
                return new StorehouseMovementGroupDTO<StorehouseMovementItemDTO>() {
                    ModelKey = key,
                    ModelInfo = new ModelInfoDTO {
                        Articul = groupedItemsList.First().Articul,
                        Name = groupedItemsList.First().Name,
                        Price = groupedItemsList.First().Price,
                        PriceInRub = groupedItemsList.First().PriceInRub,
                        ModelId = groupedItemsList.First().ModelId,
                        Size = groupedItemsList.First().Size,
                        MainPhoto = groupedItemsList.First().MainPhoto
                    },
                    Items = groupedItemsList.GroupBy(c => new {c.ModelId, c.Size, c.ColorId})
                        .Select(x => new StorehouseMovementItemDTO {
                            Color = new ColorDTO {
                                Code = x.First().Color.Group?.Hex ?? "",
                                Name = x.First().Color.Name,
                                Id = x.Key.ColorId,
                                Image = ""
                            },
                            Size = x.Key.Size,
                            InitialAmount = x.Where(c => c.Type == TimePeriodType.BeforePeriodStarted).Sum(c => c.Amount * c.Direction.GetMovementDirectionMultiplier()),
                            IncomeAmount = x.Where(c => c.Type == TimePeriodType.AfterPeriodStarted && c.Direction == MovementDirection.In).Sum(c => c.Amount),
                            WriteOffAmount = x.Where(c => c.Type == TimePeriodType.AfterPeriodStarted && c.Direction == MovementDirection.Out).Sum(c => c.Amount),
                            CurrentAmount = x.Sum(c => c.Amount * c.Direction.GetMovementDirectionMultiplier()),
                        })
                        .OrderBy(x=>x.Color.Name)
                        .ThenBy(x=>x.Size.Value)
                        .ToList()
                };
            }).ToList();
            return result;
        }
        
        public async Task<List<StorehouseMovementGroupDTO<StorehouseMovementItemDTO>>> GetModelMovementAsync(int userId, int modelId, DateTimeOffset fromDate, DateTimeOffset toDate) {
            var items = await _uow.PartnerDocuments.GetModelMovementAsync(userId, modelId);
        
            var result = items.GroupBy(x => x.GetItemKey(), (key, groupedItems) => {
                    var groupedItemsList = groupedItems.ToList();
                    var first = groupedItemsList.First();
                    return new StorehouseMovementGroupDTO<StorehouseMovementItemDTO> {
                        ModelKey = key,
                        ModelInfo = new ModelInfoDTO {
                            Articul = groupedItemsList.First().Articul,
                            Name = groupedItemsList.First().Model.Name,
                            Price = groupedItemsList.First().Price,
                            PriceInRub = groupedItemsList.First().PriceInRub,
                            ModelId = groupedItemsList.First().ModelId,
                            Size = new SizeValue(groupedItemsList.First().Model.SizeLine),
                            MainPhoto = groupedItemsList.First().Model.Descriptor.MainPhoto
                        },
                        Items = new List<StorehouseMovementItemDTO> {
                            new() {
                                Color = new ColorDTO {
                                    Code = first.Color.Group?.Hex ?? "",
                                    Name = first.Color.Name,
                                    Id = first.Color.Id,
                                    Image = ""
                                },
                                Size = first.Size,
                                InitialAmount = groupedItemsList.Where(c => c.Document.CreateDate < fromDate).Sum(c => c.Amount * c.Document.Direction.GetMovementDirectionMultiplier()),
                                IncomeAmount = groupedItemsList.Where(c => c.Document.CreateDate >= fromDate && c.Document.CreateDate <= toDate && c.Document.Direction == MovementDirection.In).Sum(c => c.Amount),
                                WriteOffAmount = groupedItemsList.Where(c => c.Document.CreateDate >= fromDate && c.Document.CreateDate <= toDate && c.Document.Direction == MovementDirection.Out).Sum(c => c.Amount),
                                CurrentAmount = groupedItemsList.Sum(c => c.Amount * c.Document.Direction.GetMovementDirectionMultiplier())
                            }
                        },
                        Documents = groupedItemsList.Where(x => x.Document.CreateDate >= fromDate).Select(x => {
                            var document = x.Document.Parent ?? x.Document;
                            return new DocumentMovementItemDTO {
                                Name = document.Name,
                                Number = document.Number,
                                CreateDate = document.CreateDate,
                                InitialAmount = groupedItemsList.Where(c => c.Document.CreateDate < x.Document.CreateDate)
                                    .Sum(c => c.Amount * c.Document.Direction.GetMovementDirectionMultiplier()),
                                IncomeAmount = x.Document.Direction == MovementDirection.In ? x.Amount : 0,
                                WriteOffAmount = x.Document.Direction == MovementDirection.Out ? x.Amount : 0,
                                CurrentAmount =
                                    groupedItemsList.Where(c => c.Document.CreateDate < x.Document.CreateDate).Sum(c =>
                                        c.Amount * c.Document.Direction.GetMovementDirectionMultiplier()) +
                                    x.Amount * x.Document.Direction.GetMovementDirectionMultiplier()
                            };
                        }).OrderBy(x => x.Number).ToList()
                    };
                }).OrderBy(x => x.Items.First().Color.Name)
                .ThenBy(x => x.Items.First().Size.Value)
                .ToList();
        
             return result;
        }
    }
}