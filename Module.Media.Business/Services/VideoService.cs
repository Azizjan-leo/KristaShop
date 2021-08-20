using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KristaShop.Common.Models;
using KristaShop.DataAccess.Entities.Media;
using Microsoft.EntityFrameworkCore;
using Module.Common.Business.Interfaces;
using Module.Media.Business.DTOs;
using Module.Media.Business.Interfaces;
using Module.Media.Business.UnitOfWork;
using P.Pager;

namespace Module.Media.Business.Services {
    public class VideoService : IVideoService {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly IFileService _fileService;

        public VideoService(IMapper mapper, IUnitOfWork uow, IFileService fileService) {
            _mapper = mapper;
            _uow = uow;
            _fileService = fileService;
        }

        public async Task<List<VideoDTO>> GetVideosAsync(bool onlyVisible = true) {
            return await _mapper
                .ProjectTo<VideoDTO>(_uow.VideoGalleryVideos.GetAllOrderedOnlyVisibleVideos(onlyVisible)).ToListAsync();
        }

        public async Task<IPager<VideoDTO>> GetVideosOnPageAsync(int page, int quantity, bool onlyVisible = true) {
            return await _mapper
                .ProjectTo<VideoDTO>(_uow.VideoGalleryVideos.GetAllOrderedOnlyVisibleVideos(onlyVisible))
                .ToPagerListAsync(page, quantity);
        }

        public async Task<List<VideoDTO>> GetVideosByGalleryAsync(Guid galleryId, bool onlyVisible = true) {
            return await _mapper
                .ProjectTo<VideoDTO>(_uow.VideoGalleryVideos.GetInGalleryOnlyVisibleOrdered(galleryId, onlyVisible))
                .ToListAsync();
        }

        public async Task<IPager<VideoDTO>> GetVideosByGalleryOnPageAsync(Guid galleryId, int page, int quantity = 20,
            bool onlyVisible = true) {
            return await _mapper
                .ProjectTo<VideoDTO>(_uow.VideoGalleryVideos.GetInGalleryOnlyVisibleOrdered(galleryId, onlyVisible))
                .ToPagerListAsync(page, quantity);
        }

        public async Task<VideoDTO> GetVideoAsync(Guid id, bool onlyVisible = true) {
            var request = _uow.Videos.All;

            if (onlyVisible) {
                request = request.Where(x => x.IsVisible);
            }

            return await _mapper.ProjectTo<VideoDTO>(request).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<OperationResult> InsertVideoAsync(VideoDTO video) {
            try {
                var entity = _mapper.Map<Video>(video);

                if (video.Preview != null) {
                    entity.PreviewPath = await _fileService.SaveFileAsync(video.Preview);
                    if (string.IsNullOrEmpty(entity.PreviewPath)) {
                        return _fileService.GetLastError();
                    }
                }

                await _uow.BeginTransactionAsync();
                await _uow.Videos.AddAsync(entity, true);
                await _addVideoToGalleriesAsync(video.GalleryIds, entity.Id);

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

        public async Task<OperationResult> UpdateVideoAsync(VideoDTO video) {
            try {
                var entity = await _uow.Videos.GetByIdAsync(video.Id);
                if (entity == null) {
                    return OperationResult.Failure();
                }

                if (video.Preview != null) {
                    if (!string.IsNullOrEmpty(entity.PreviewPath)) {
                        if (!_fileService.RemoveFile(video.Preview.FilesDirectoryPath, entity.PreviewPath)) {
                            return _fileService.GetLastError();
                        }
                    }

                    video.PreviewPath = await _fileService.SaveFileAsync(video.Preview);
                    if (string.IsNullOrEmpty(video.PreviewPath)) {
                        return _fileService.GetLastError();
                    }
                }

                await _uow.BeginTransactionAsync();
                entity = _mapper.Map(video, entity);
                _uow.Videos.Update(entity);
                await _updateVideoGalleriesAsync(entity.Id, video.GalleryIds);

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

        public async Task<OperationResult> DeleteVideoAsync(Guid id, string fileDirectoryPath) {
            try {
                var entity = await _uow.Videos.GetByIdAsync(id);
                if (entity == null) {
                    return OperationResult.Failure();
                }

                _fileService.RemoveFile(fileDirectoryPath, entity.PreviewPath);

                await _uow.BeginTransactionAsync();
                await _removeVideoFromGalleriesAsync(entity.Id);
                _uow.Videos.Delete(entity);

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

        public async Task<OperationResult> UpdateVideoOrder(Guid galleryId, Guid videoId, int order) {
            try {
                await _uow.BeginTransactionAsync();
                var video = await _uow.VideoGalleryVideos.GetByGalleryAndVideoIdAsync(galleryId, videoId);
                if (video != null) {
                    video.Order = order;
                }

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

        public async Task<OperationResult> RestoreVideosOrderAsync() {
            try {
                await _uow.BeginTransactionAsync();
                var galleries = await _uow.VideoGalleryVideos.All.GroupBy(x => x.GalleryId).ToListAsync();

                foreach (var gallery in galleries) {
                    var order = 0;
                    foreach (var video in gallery) {
                        video.Order = ++order;
                    }

                    _uow.VideoGalleryVideos.UpdateRange(gallery.ToList());
                }

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

        private async Task _addVideoToGalleriesAsync(IEnumerable<Guid> galleryIds, Guid videoId) {
            foreach (var galleryId in galleryIds.Distinct()) {
                var order = await _uow.VideoGalleryVideos.GetNewOrderValueAsync(galleryId);
                await _uow.VideoGalleryVideos.AddAsync(new VideoGalleryVideos(true)
                    {GalleryId = galleryId, VideoId = videoId, Order = order});
            }
        }

        private async Task _removeVideoFromGalleriesAsync(Guid videoId, IEnumerable<Guid> galleryIds = null) {
            var request = _uow.VideoGalleryVideos.All.Where(x => x.VideoId == videoId);

            if (galleryIds != null) {
                request = request.Where(x => galleryIds.Contains(x.GalleryId));
            }

            var videos = await request.ToListAsync();
            _uow.VideoGalleryVideos.DeleteRange(videos);

            foreach (var video in videos) {
                await _restoreVideosOrderFromPositionAsync(video.Order, video.GalleryId);
            }
        }

        private async Task _updateVideoGalleriesAsync(Guid videoId, List<Guid> galleryIds) {
            var existingGalleryIds = await _uow.VideoGalleryVideos.All
                .Where(x => x.VideoId == videoId)
                .Select(x => x.GalleryId)
                .ToListAsync();

            var toRemove = existingGalleryIds.Where(x => !galleryIds.Contains(x));
            var toAdd = galleryIds.Where(x => !existingGalleryIds.Contains(x));

            await _addVideoToGalleriesAsync(toAdd, videoId);
            await _removeVideoFromGalleriesAsync(videoId, toRemove);
        }

        private async Task _restoreVideosOrderFromPositionAsync(int orderPosition, Guid galleryId) {
            var videos = await _uow.VideoGalleryVideos.GetAllOrderedByGallery(galleryId)
                .Where(x => x.Order > orderPosition).ToListAsync();
            var order = orderPosition - 1;
            foreach (var video in videos) {
                video.Order = ++order;
            }
        }
    }
}