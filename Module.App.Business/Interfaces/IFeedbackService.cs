using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Models;
using Module.App.Business.Models;

namespace Module.App.Business.Interfaces {
    public interface IFeedbackService {
        Task<int> GetNewFeedbacksCount();
        Task<FeedbackDTO> GetFeedbackDetails(Guid id);
        Task<List<FeedbackDTO>> GetFeedbackListAsync();
        Task<OperationResult> InsertFeedback(FeedbackDTO feedback);
        Task<OperationResult> InsertFeedbackWithFileAsync(FeedbackDTO feedback, FeedbackCreateFileDTO createFile);
        Task<OperationResult> UpdateFeedback(Guid id, Guid userId);
        Task<List<FeedbackFileDTO>> GetFilesByFeedbackIdAsync(Guid id);
        Task<FeedbackFileDTO> GetFileAsync(Guid id);
        Task<OperationResult> DeleteFeedbackAsync(Guid id);
        Task<OperationResult> DeleteFeedbackAsync(bool viewedOnly = true);
    }
}