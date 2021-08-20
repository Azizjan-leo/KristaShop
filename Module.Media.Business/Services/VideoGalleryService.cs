using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using KristaShop.Common.Interfaces;
using KristaShop.Common.Models;
using KristaShop.DataAccess.Entities.Media;
using Microsoft.EntityFrameworkCore;
using Module.Common.Business.Interfaces;
using Module.Media.Business.DTOs;
using Module.Media.Business.Interfaces;
using Module.Media.Business.UnitOfWork;
using P.Pager;

namespace Module.Media.Business.Services {
    /// <summary>
    /// This class implements IVideoGalleryService
    /// It includes VideoService <see cref="VideoService"/> to implement IVideoService interface
    /// </summary>
    public class VideoGalleryService : IVideoGalleryService {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly IVideoService _videoService;
        private readonly IFileService _fileService;

        public VideoGalleryService(IMapper mapper, IUnitOfWork uow, IVideoService videoService,
            IFileService fileService) {
            _mapper = mapper;
            _uow = uow;
            _videoService = videoService;
            _fileService = fileService;
        }

        #region IVideoGalleryService implementation

        public async Task<OperationResult> SwitchGalleryVisabilityAsync(Guid id) {
            var item = await _uow.VideoGalleries.GetByIdAsync(id);
            if (item == null) return OperationResult.Failure();
            item.IsVisible = !item.IsVisible;
            await _uow.SaveAsync();
            return OperationResult.Success();
        }

        public async Task<OperationResult> SwitchVideoVisabilityAsync(Guid id) {
            var item = await _uow.Videos.GetByIdAsync(id);
            if (item == null) return OperationResult.Failure();
            item.IsVisible = !item.IsVisible;
            await _uow.SaveAsync();
            return OperationResult.Success();
        }

        public async Task<List<VideoGalleryDTO>> GetGalleriesAsync(bool openOnly = false, bool onlyVisible = true) {
            return await _uow.VideoGalleries.GetAllOnlyVisibleAndOpenOrdered(onlyVisible, openOnly)
                .ProjectTo<VideoGalleryDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<VideoGalleryDTO> GetGalleryAsync(Guid id, bool openOnly = false, bool onlyVisible = true) {
            return await _uow.VideoGalleries.GetAllOnlyVisibleAndOpenOrdered(onlyVisible, openOnly)
                .ProjectTo<VideoGalleryDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<VideoGalleryDTO>
            GetGalleryAsync(string slug, bool openOnly = false, bool onlyVisible = true) {
            return await _uow.VideoGalleries.GetAllOnlyVisibleAndOpenOrdered(onlyVisible, openOnly)
                .ProjectTo<VideoGalleryDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Slug.Equals(slug));
        }

        public async Task<List<VideoGalleryWithVideosDTO>> GetGalleriesWithVideoAsync(int videosQuantity,
            bool openOnly = false, bool onlyVisible = true) {
            return await _uow.VideoGalleries.GetAllOnlyVisibleAndOpenOrdered(onlyVisible, openOnly)
                .ProjectTo<VideoGalleryWithVideosDTO>(_mapper.ConfigurationProvider, new {quantity = videosQuantity})
                .ToListAsync();
        }

        public async Task<VideoGalleryWithVideosDTO> GetGalleryWithVideoAsync(Guid id, int videosQuantity,
            bool openOnly = false, bool onlyVisible = true) {
            return await _uow.VideoGalleries.GetAllOnlyVisibleAndOpenOrdered(onlyVisible, openOnly)
                .ProjectTo<VideoGalleryWithVideosDTO>(_mapper.ConfigurationProvider, new {quantity = videosQuantity})
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<VideoGalleryWithVideosDTO> GetGalleryWithVideoAsync(string slug, int videosQuantity,
            bool openOnly = false, bool onlyVisible = true) {
            return await _uow.VideoGalleries.GetAllOnlyVisibleAndOpenOrdered(onlyVisible, openOnly)
                .Include(x => x.VideoGalleryVideos)
                .ProjectTo<VideoGalleryWithVideosDTO>(_mapper.ConfigurationProvider, new {quantity = videosQuantity})
                .FirstOrDefaultAsync(x => x.Slug.Equals(slug));
        }

        public async Task<VideoGalleryWithVideosDTO> GetFirstGalleryWithVideosAsync(int videosQuantity,
            bool openOnly = false, bool onlyVisible = true) {
            return await _uow.VideoGalleries.GetAllOnlyVisibleAndOpenOrdered(onlyVisible, openOnly)
                .Include(x => x.VideoGalleryVideos)
                .ProjectTo<VideoGalleryWithVideosDTO>(_mapper.ConfigurationProvider, new {quantity = videosQuantity})
                .FirstOrDefaultAsync();
        }

        public async Task<OperationResult> InsertGalleryAsync(VideoGalleryDTO gallery) {
            var entity = _mapper.Map<VideoGallery>(gallery);
            entity.Order = await _getNewOrderValueAsync();

            if (gallery.Preview != null) {
                entity.PreviewPath = await _fileService.SaveFileAsync(gallery.Preview);
                if (string.IsNullOrEmpty(entity.PreviewPath)) {
                    return _fileService.GetLastError();
                }
            }

            await _uow.VideoGalleries.AddAsync(entity, true);
            if (!await _uow.SaveAsync()) {
                return OperationResult.Failure();
            }

            return OperationResult.Success();
        }

        public async Task<OperationResult> UpdateGalleryAsync(VideoGalleryDTO gallery) {
            var entity = await _uow.VideoGalleries.GetByIdAsync(gallery.Id);
            if (entity == null) {
                return OperationResult.Failure();
            }

            if (gallery.Preview != null) {
                if (!string.IsNullOrEmpty(entity.PreviewPath)) {
                    if (!_fileService.RemoveFile(gallery.Preview.FilesDirectoryPath, entity.PreviewPath)) {
                        return _fileService.GetLastError();
                    }
                }

                gallery.PreviewPath = await _fileService.SaveFileAsync(gallery.Preview);
                if (string.IsNullOrEmpty(gallery.PreviewPath)) {
                    return _fileService.GetLastError();
                }
            }

            entity = _mapper.Map(gallery, entity);
            _uow.VideoGalleries.Update(entity);
            if (!await _uow.SaveAsync()) {
                return OperationResult.Failure();
            }

            return OperationResult.Success();
        }

        public async Task<OperationResult> DeleteGalleryAsync(Guid id, string filesDirectoryPath) {
            var entity = await _uow.VideoGalleries.GetByIdAsync(id);
            if (entity == null) {
                return OperationResult.Failure();
            }

            _fileService.RemoveFile(filesDirectoryPath, entity.PreviewPath);

            _uow.VideoGalleries.Delete(entity);
            if (!await _uow.SaveAsync()) {
                return OperationResult.Failure();
            }

            return OperationResult.Success();
        }

        public async Task<OperationResult> RestoreGalleriesOrderAsync() {
            try {
                await _uow.BeginTransactionAsync();
                var videos = await _uow.VideoGalleries.All.OrderBy(x => x.Order).ToListAsync();
                for (int i = 0; i < videos.Count; i++) {
                    videos[i].Order = i + 1;
                }

                _uow.VideoGalleries.UpdateRange(videos);
                if (!await _uow.SaveAsync()) {
                    return OperationResult.Failure();
                }

                _uow.CommitTransaction();
                return OperationResult.Success();
            } catch (Exception) {
                _uow.RollbackTransaction();
                throw;
            }
        }

        private async Task<int> _getNewOrderValueAsync() {
            return await _uow.VideoGalleries.All.MaxAsync(x => (int?) x.Order) + 1 ?? 1;
        }

        #endregion

        #region IVideoService implementation

        public async Task<List<VideoDTO>> GetVideosAsync(bool onlyVisible = true) {
            return await _videoService.GetVideosAsync(onlyVisible);
        }

        public async Task<IPager<VideoDTO>> GetVideosOnPageAsync(int page, int quantity, bool onlyVisible = true) {
            return await _videoService.GetVideosOnPageAsync(page, quantity, onlyVisible);
        }

        public async Task<List<VideoDTO>> GetVideosByGalleryAsync(Guid galleryId, bool onlyVisible = true) {
            return await _videoService.GetVideosByGalleryAsync(galleryId, onlyVisible);
        }

        public async Task<IPager<VideoDTO>> GetVideosByGalleryOnPageAsync(Guid galleryId, int page, int quantity = 20,
            bool onlyVisible = true) {
            return await _videoService.GetVideosByGalleryOnPageAsync(galleryId, page, quantity, onlyVisible);
        }

        public async Task<VideoDTO> GetVideoAsync(Guid id, bool onlyVisible = true) {
            return await _videoService.GetVideoAsync(id, onlyVisible);
        }

        public async Task<OperationResult> InsertVideoAsync(VideoDTO video) {
            return await _videoService.InsertVideoAsync(video);
        }

        public async Task<OperationResult> UpdateVideoAsync(VideoDTO video) {
            return await _videoService.UpdateVideoAsync(video);
        }

        public async Task<OperationResult> DeleteVideoAsync(Guid id, string fileDirectoryPath) {
            return await _videoService.DeleteVideoAsync(id, fileDirectoryPath);
        }

        public async Task<OperationResult> UpdateVideoOrder(Guid galleryId, Guid videoId, int order) {
            return await _videoService.UpdateVideoOrder(galleryId, videoId, order);
        }

        public async Task<OperationResult> RestoreVideosOrderAsync() {
            return await _videoService.RestoreVideosOrderAsync();
        }

        #endregion
    }
}