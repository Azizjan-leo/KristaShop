using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Module.Media.Business.DTOs;

namespace Module.Media.Business.Interfaces {
    public interface IFaqService {
        List<FaqDTO> GetAllFaqs();
        Task CreateFaqAsync(FaqDTO model);
        FaqDTO GetFaqById(Guid faqId);
        FaqSectionDto GetFaqSectionById(Guid id);
        Task<Guid> CreateFaqSectionAsync(FaqSectionDto model, string faqDocumentsDirectory, string rootDirectory);
        Task<Guid> CreateFaqSectionContentAsync(FaqSectionContentDto model, string faqDocumentsDirectory, string rootDirectory);
        Task<List<FaqSectionDto>> GetFaqSectionsAsync(Guid id);
        Task<List<FaqSectionContentDto>> GetFaqSectionContentAsync(Guid id);
        Task<string> GetFaqColorCode(Guid faqId);
        Task DeleteFaqAsync(Guid id);
        Task<Guid> DeleteFaqSectionAsync(Guid id);
        Task<Guid> DeleteFaqContentAsync(Guid id);
        Task UpdateFaqAsync(FaqDTO model);
        Task<Guid> UpdateFaqSectionAsync(FaqSectionDto model, string faqDocumentsDirectory, string rootDirectory);
        Task<Guid> UpdateFaqSectionContentAsync(FaqSectionContentDto model, string faqDocumentsDirectory, string rootDirectory);
        Task<FaqSectionContentDto> GetFaqSectionContentByIdAsync(Guid id);
    }
}
