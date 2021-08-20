using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KristaShop.Common.Models;
using KristaShop.DataAccess.Domain;
using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Module.Catalogs.Business.Interfaces;
using Module.Catalogs.Business.Models;
using Serilog;

namespace Module.Catalogs.Business.Services {
    public class Favorite1CService : IFavorite1CService
    {
        private readonly KristaShopDbContext _dbContext;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public Favorite1CService(KristaShopDbContext dbContext, ILogger logger, IMapper mapper) {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CatalogItemBriefDTO>> getUserFavoritesAsync(int userId) {
            var itemsList = await _dbContext.Favorite1CItemItems
                .Where(x => x.UserId == userId)
                .Join(
                    _dbContext.CatalogItemDescriptor,
                    p => p.Articul,
                    c => c.Articul,
                    (p, c) => new CatalogItemBriefDTO() {
                        Articul = p.Articul,
                        CatalogId = p.CatalogId,
                        MainPhoto = c.MainPhoto,
                        AltText = c.AltText
                    }
                )
                .OrderBy(x => x.CatalogId)
                .ThenBy(x => x.Articul)
                .ToListAsync();

            return itemsList;
        }

        public async Task<bool> IsFavoriteAsync(string articul, int catalogId, int userId) {
            var favorite = await _dbContext.Favorite1CItemItems.Where(x => x.UserId == userId && x.Articul == articul && x.CatalogId == catalogId).FirstOrDefaultAsync();
            
            return favorite != null;
        }

        public async Task<OperationResult> AddOrDeleteFavoriteAsync(int userId, string articul, int catalogId) {
            bool wasAdded = false;
            try {
                OperationResult or;
                var favorite = await _dbContext.Favorite1CItemItems.Where(x => x.UserId == userId && x.Articul == articul && x.CatalogId == catalogId).FirstOrDefaultAsync();
                if (favorite == null) {
                    favorite = new Favorite1CItemItem() {
                        UserId = userId,
                        Articul = articul,
                        CatalogId = catalogId
                    };

                    _dbContext.Add(favorite);

                    wasAdded = true;
                    or = OperationResult.Success("Данная модель успешно добавлена в избранное.");
                } else {
                    _dbContext.Remove(favorite);

                    or = OperationResult.Success("Данная модель успешно удалена из избранного.");
                }

                await _dbContext.SaveChangesAsync();

                return or;
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to change user favorites. {message}", ex.Message);
                
                return wasAdded ? OperationResult.Failure("Не удалось добавить модель в избранное.") : OperationResult.Failure("Не удалось удалить модель из избранного.");
            }
        }

        public async Task<OperationResult> DeleteFavoriteAsync(int userId, string articul, int catalogId) {
            try {
                var favorite = await _dbContext.Favorite1CItemItems.Where(x => x.UserId == userId && x.Articul == articul && x.CatalogId == catalogId).FirstOrDefaultAsync();
                if (favorite != null) {
                    _dbContext.Remove(favorite);
                    await _dbContext.SaveChangesAsync();
                }

                return OperationResult.Success("Данная модель успешно удалена из избранного.");
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to delete user favorites. {message}", ex.Message);

                return OperationResult.Failure("Не удалось удалить модель из избранного.");
            }
        }

    }
}