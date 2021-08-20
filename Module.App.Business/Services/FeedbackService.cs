using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using KristaShop.Common.Enums;
using KristaShop.Common.Extensions;
using KristaShop.Common.Models;
using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Module.App.Business.Interfaces;
using Module.App.Business.Models;
using Module.App.Business.UnitOfWork;
using Module.Common.Business.Interfaces;
using Serilog;

namespace Module.App.Business.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly ILogger _logger;
        private readonly GlobalSettings _settings;

        public FeedbackService(IUnitOfWork uow, IMapper mapper, IFileService fileService, ILogger logger, IOptions<GlobalSettings> settings) {
            _uow = uow;
            _mapper = mapper;
            _fileService = fileService;
            _logger = logger;
            _settings = settings.Value;
        }

        public async Task<List<FeedbackDTO>> GetFeedbackListAsync() {
            return await _uow.Feedback.All
                .Select(x => new FeedbackDTO {
                    Id = x.Id,
                    Phone = x.Phone,
                    Person = x.Person,
                    Message = x.Message,
                    Email = x.Email,
                    Viewed = x.Viewed,
                    RecordTimeStamp = x.RecordTimeStamp,
                    FormattedDate = x.RecordTimeStamp.ToBasicString(),
                    Type = x.Type,
                    FeedbackType = x.Type.ToReadableString(),
                    FilesCount = x.Files.Count
                })
                .OrderBy(x => x.Viewed)
                .ThenByDescending(x => x.RecordTimeStamp)
                .ToListAsync();
        }

        public async Task<FeedbackDTO> GetFeedbackDetails(Guid id) {
            var entity = await _uow.Feedback.GetByIdAsync(id);
            return _mapper.Map<FeedbackDTO>(entity);
        }

        public async Task<OperationResult> InsertFeedback(FeedbackDTO feedback) {
            var entity = _mapper.Map<Feedback>(feedback);
            await _uow.Feedback.AddAsync(entity, true);

            return (await _uow.SaveAsync()) ? OperationResult.Success() : OperationResult.Failure();
        }

        public async Task<OperationResult> InsertFeedbackWithFileAsync(FeedbackDTO feedback, FeedbackCreateFileDTO createFile) {
            var filepath = string.Empty;
            try {
                await _uow.BeginTransactionAsync();
                createFile.OriginalName = createFile.OriginalName.ToValidFileName();
                filepath = await _fileService.SaveFileAsync(createFile);
                if (string.IsNullOrEmpty(filepath)) {
                    return OperationResult.Failure("Не удалось сохранить файл");
                }

                var feedbackEntity = _mapper.Map<Feedback>(feedback);
                await _uow.Feedback.AddAsync(feedbackEntity, true);
                if (!await _uow.SaveAsync()) {
                    return OperationResult.Failure("Не удалось сохранить сообщение обратной связи");
                }

                var fileEntity = _mapper.Map<FeedbackFile>(createFile);
                fileEntity.ParentId = feedbackEntity.Id;
                fileEntity.VirtualPath = filepath;
                await _uow.FeedbackFiles.AddAsync(fileEntity, true);
                if (!await _uow.SaveAsync()) {
                    return OperationResult.Failure("Не удалось сохранить данные файла обратной связи");
                }

                _uow.CommitTransaction();
                return OperationResult.Success();
            } catch (Exception ex) {
                _uow.RollbackTransaction();
                _fileService.RemoveFile(createFile.FilesDirectoryPath, filepath);
                _logger.Error(ex, "Failed to insert feedback with file. {message}", ex.Message);
                return OperationResult.Failure();
            }
        }

        public async Task<OperationResult> UpdateFeedback(Guid id, Guid userId) {
            var entity = await _uow.Feedback.GetByIdAsync(id);
            entity.Viewed = true;
            entity.ViewTimeStamp = DateTime.Now;
            entity.ReviewerUserId = userId;
            _uow.Feedback.Update(entity);

            return (await _uow.SaveAsync()) ? OperationResult.Success() : OperationResult.Failure();
        }

        public async Task<List<FeedbackFileDTO>> GetFilesByFeedbackIdAsync(Guid id) {
            return await _uow.FeedbackFiles.All.Where(x => x.ParentId == id)
                .ProjectTo<FeedbackFileDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<FeedbackFileDTO> GetFileAsync(Guid id) {
            var file = await _uow.FeedbackFiles.GetByIdAsync(id);
            return file != null ? _mapper.Map<FeedbackFileDTO>(file) : null;
        }

        public async Task<OperationResult> DeleteFeedbackAsync(Guid id) {
            try {
                await _uow.BeginTransactionAsync();
                var feedback = await _uow.Feedback.All.Include(x => x.Files).FirstOrDefaultAsync(x => x.Id == id);

                if (feedback == null) {
                    return OperationResult.Success();
                }

                await _deleteFeedbackFilesAsync(feedback.Files);
                _uow.Feedback.Delete(feedback);
                await _uow.SaveAsync();
                _uow.CommitTransaction();
                return OperationResult.Success();
            } catch (Exception) {
                _uow.RollbackTransaction();
                throw;
            }
        }

        public async Task<OperationResult> DeleteFeedbackAsync(bool viewedOnly = true) {
            try {
                await _uow.BeginTransactionAsync();
                var query = _uow.Feedback.All.Include(x => x.Files).AsQueryable();

                if (viewedOnly) {
                    query = query.Where(x => x.Viewed);
                }

                var feedbackItems = await query.ToListAsync();

                if (!feedbackItems.Any()) {
                    return OperationResult.Success();
                }

                foreach (var feedback in feedbackItems) {
                    await _deleteFeedbackFilesAsync(feedback.Files);
                }
                
                _uow.Feedback.DeleteRange(feedbackItems);
                await _uow.SaveAsync();
                _uow.CommitTransaction();
                return OperationResult.Success();
            } catch (Exception) {
                _uow.RollbackTransaction();
                throw;
            }
        }

        private async Task _deleteFeedbackFilesAsync(ICollection<FeedbackFile> files) {
            _uow.FeedbackFiles.DeleteRange(files);
            foreach (var file in files) {
                _fileService.RemoveFile(_settings.FilesDirectoryPath, file.VirtualPath);
            }

            await _uow.SaveAsync();
        }

        public async Task<int> GetNewFeedbacksCount()  => await _uow.Feedback.All.Where(x => !x.Viewed).CountAsync();
    }
}