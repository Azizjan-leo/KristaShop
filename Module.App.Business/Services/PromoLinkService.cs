using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using KristaShop.Common.Extensions;
using KristaShop.Common.Models;
using KristaShop.Common.Models.Session;
using KristaShop.DataAccess.Domain;
using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Module.App.Business.Interfaces;
using Module.App.Business.Models;
using Module.Common.Business.Interfaces;

namespace Module.App.Business.Services {
    public class PromoLinkService : IPromoLinkService {
        private readonly KristaShopDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly GlobalSettings _globalSettings;

        public PromoLinkService(KristaShopDbContext context, IMapper mapper, IFileService fileService, IOptions<GlobalSettings> settings) {
            _context = context;
            _mapper = mapper;
            _fileService = fileService;
            _globalSettings = settings.Value;
        }

        public async Task<List<PromoLinkDTO>> GetPromoLinksAsync(UserSession userSession) {
            var request = _context.PromoLinks.AsQueryable();
            if (userSession.IsManager) {
                request = request.Where(x => x.ManagerId == userSession.ManagerId);
            }

            return await request
                .ProjectTo<PromoLinkDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<PromoLinkDTO> GetPromoLinkAsync(Guid promoLinkId) {
            var entity = await _context.PromoLinks.FindAsync(promoLinkId);
            return entity == null ? null : _mapper.Map<PromoLinkDTO>(entity);
        }

        public async Task<OperationResult> InsertPromoLinkAsync(PromoLinkDTO promoLink) {
            var entity = _mapper.Map<PromoLink>(promoLink);
            entity.Id = Guid.NewGuid();
            entity.Link = entity.Link.ToValidLatinString();

            if (promoLink.VideoPreview != null) {
                entity.VideoPreviewPath = await _fileService.SaveFileAsync(promoLink.VideoPreview);
                if (string.IsNullOrEmpty(entity.VideoPreviewPath)) {
                    return _fileService.GetLastError();
                }
            }

            await _context.PromoLinks.AddAsync(entity);
            await _context.SaveChangesAsync();
            return OperationResult.Success();
        }

        public async Task<OperationResult> UpdatePromoLinkAsync(PromoLinkDTO promoLink) {
            var entity = await _context.PromoLinks.FindAsync(promoLink.Id);
            if (entity == null) {
                return OperationResult.Failure("Данная промо ссылка не существует");
            }

            _mapper.Map(promoLink, entity);
            if (promoLink.VideoPreview != null) {
                if (!string.IsNullOrEmpty(entity.VideoPreviewPath)) {
                    if (!_fileService.RemoveFile(promoLink.VideoPreview.FilesDirectoryPath, entity.VideoPreviewPath)) {
                        return _fileService.GetLastError();
                    }
                }

                entity.VideoPreviewPath = await _fileService.SaveFileAsync(promoLink.VideoPreview);
                if (string.IsNullOrEmpty(entity.VideoPreviewPath)) {
                    return _fileService.GetLastError();
                }
            }

            entity.Link = entity.Link.ToValidLatinString();
            _context.PromoLinks.Update(entity);
            await _context.SaveChangesAsync();
            return OperationResult.Success();
        }

        public async Task<OperationResult> DeletePromoLinkAsync(Guid promoLinkId) {
            var entity = await _context.PromoLinks.FindAsync(promoLinkId);
            if (entity == null) {
                return OperationResult.Success();
            }

            _fileService.RemoveFile(_globalSettings.FilesDirectoryPath, entity.VideoPreviewPath);

            _context.PromoLinks.Remove(entity);
            await _context.SaveChangesAsync();
            return OperationResult.Success();
        }
    }
}
