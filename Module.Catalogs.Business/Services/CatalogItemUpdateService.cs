using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KristaShop.Common.Enums;
using KristaShop.Common.Exceptions;
using KristaShop.DataAccess.Entities;
using Module.Catalogs.Business.Interfaces;
using Module.Catalogs.Business.Models;
using Module.Catalogs.Business.UnitOfWork;

namespace Module.Catalogs.Business.Services {
    public class CatalogItemUpdateService : ICatalogItemUpdateService {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CatalogItemUpdateService(IUnitOfWork uow, IMapper mapper) {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task UpdateModelDescriptionAsync(UpdateCatalogItemDescriptorDTO descriptor) {
            await using (await _uow.BeginTransactionAsync()) {
                var entity = await _uow.CatalogItemDescriptors.GetByIdAsync(descriptor.Articul);
                if (entity == null) {
                    entity = _mapper.Map<CatalogItemDescriptor>(descriptor);
                    await _uow.CatalogItemDescriptors.AddAsync(entity, true);
                } else {
                    if (!string.IsNullOrEmpty(descriptor.MainPhoto)) {
                        entity.MainPhoto = descriptor.MainPhoto;
                    }

                    entity.IsVisible = descriptor.IsVisible;
                    entity.AddDate = descriptor.AddDate ?? DateTime.Now;
                    entity.Description = descriptor.Description;
                    entity.Matherial = descriptor.Material;
                    entity.AltText = descriptor.AltText;
                    entity.VideoLink = descriptor.VideoLink;
                    entity.MetaTitle = descriptor.MetaTitle;
                    entity.MetaKeywords = descriptor.MetaKeywords;
                    entity.MetaDescription = descriptor.MetaDescription;
                    entity.IsLimited = descriptor.IsLimited;
                    
                    _uow.CatalogItemDescriptors.Update(entity);
                }

                if (descriptor.UploadedPhotos is {Count: > 0}) {
                    var lastNomPhotoOrder = await _uow.ModelPhotos.GetMaxOrderNumberAsync(descriptor.Articul);
                    var newPhotos = descriptor.UploadedPhotos
                        .Select(path => new ModelPhoto1C(descriptor.Articul, path, ++lastNomPhotoOrder))
                        .ToList();
                    await _uow.ModelPhotos.AddRangeAsync(newPhotos);
                }

                await _uow.ModelCatalogInvisibilities.UpdateInvisibilityAsync(descriptor.Articul, descriptor.CatalogsInvisibility);

                await _uow.SaveChangesAsync();
                await _uow.CommitTransactionAsync();
            }
        }

        public async Task UpdateModelPhotoPositionAsync(int id, int newPosition) {
            await _uow.ModelPhotos.UpdatePhotoPosition(id, newPosition);
            await _uow.SaveChangesAsync();
        }

        public async Task SetPhotoColorAsync(int photoId, int colorId) {
            await _uow.ModelPhotos.UpdatePhotoColor(photoId, colorId);
            await _uow.SaveChangesAsync();
        }

        public async Task UpdateModelMainPhotoAsync(string articul, string path) {
            await _uow.CatalogItemDescriptors.UpdateDescriptorPhotoAsync(articul, path);
            await _uow.SaveChangesAsync();
        }

        public async Task<string> UpdateModelPhotoAsync(int photoId, string path) {
            var previousPath = await _uow.ModelPhotos.UpdatePhotoPath(photoId, path);
            await _uow.SaveChangesAsync();
            return previousPath;
        }

        public async Task UpdateModelPositionInCatalogAsync(CatalogType catalog, string articul, int toPosition) {
            await _uow.ModelCatalogOrder.CreateOrUpdateAsync(articul, catalog, toPosition);
            await _uow.SaveChangesAsync();
        }

        public async Task ReorderModelPhotosAsync(string articul, int photoId, int newPosition) {
            if (newPosition <= 0) {
                throw new Exception("Invalid ToPosition value");
            }

            var modelPhotos = (await _uow.ModelPhotos.GetAllByArticulAsync(articul)).ToList();
            var currentPhoto = modelPhotos.Find(x => x.Id == photoId);
            if (currentPhoto == null) {
                throw new EntityNotFoundException($"Model photo not found {photoId}");
            }

            await using (await _uow.BeginTransactionAsync()) {
                if (currentPhoto.Order > newPosition) {
                    var photos = modelPhotos.Where(x => x.Order >= newPosition && x.Order < currentPhoto.Order).ToList();
                    foreach (var photo in photos) {
                        photo.Order++;
                    }
                    _uow.ModelPhotos.UpdateRange(photos);
                } else {
                    var photos = modelPhotos.Where(x => x.Order > currentPhoto.Order && x.Order <= newPosition).ToList();
                    foreach (var photo in photos) {
                        photo.Order--;
                    }
                    _uow.ModelPhotos.UpdateRange(photos);
                }

                currentPhoto.Order = newPosition;
                _uow.ModelPhotos.Update(currentPhoto);

                await _uow.SaveChangesAsync();
                await _uow.CommitTransactionAsync();
            }
        }
        
        public async Task<List<string>> DeleteModelPhotoAsync(int photoId) {
            var entity = await _uow.ModelPhotos.DeleteAsync(photoId);
            await _uow.SaveChangesAsync();
            return entity != null ? new List<string> {entity.PhotoPath, entity.OldPhotoPath} : new List<string>();
        }
    }
}