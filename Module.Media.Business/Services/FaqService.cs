using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KristaShop.Common.Enums;
using KristaShop.Common.Interfaces;
using KristaShop.DataAccess.Entities.Media;
using Microsoft.EntityFrameworkCore;
using Module.Common.Business.Interfaces;
using Module.Media.Business.DTOs;
using Module.Media.Business.Interfaces;
using Module.Media.Business.UnitOfWork;

namespace Module.Media.Business.Services {
    public class FaqService : IFaqService
    {
        private readonly IUnitOfWork _uow;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public FaqService(IUnitOfWork uow, IFileService fileService, IMapper mapper)
        {
            _uow = uow;
            _fileService = fileService;
            _mapper = mapper;
        }

        public async Task CreateFaqAsync(FaqDTO model)
        {
            var newFaq = new Faq() { Title = model.Title, Id = model.Id, ColorCode = model.ColorCode };
            await _uow.FaqRepository.AddAsync(newFaq);
            await _uow.SaveAsync();
        }

        public async Task<Guid> CreateFaqSectionAsync(FaqSectionDto model, string faqDocumentsDirectory, string rootDirectory)
        {
            var newFaqSection = new FaqSection() { Title = model.Title, FaqId = model.FaqId };

            await _uow.FaqSectionRepository.AddAsync(newFaqSection);

            if (model.Icon != null)
                newFaqSection.IconUrl = await _fileService.SaveFileAsync(model.Icon, rootDirectory, $"{faqDocumentsDirectory}/{newFaqSection.Id}");
            
            await _uow.FaqSectionRepository.AddAsync(newFaqSection);
            await _uow.SaveAsync();
            return newFaqSection.FaqId;
        }

        public async Task<Guid> CreateFaqSectionContentAsync(FaqSectionContentDto model, string faqDocumentsDirectory, string rootDirectory)
        {
            var newSectionContent = new FaqSectionContent() { Content = model.Content, FaqSectionId = model.FaqSectionId };
            await _uow.FaqSectionContentRepository.AddAsync(newSectionContent);
            if (model.Image != null){
                var newSectionFile = new FaqSectionContentFile();

                newSectionFile.FileUrl = await _fileService.SaveFileAsync(model.Image, rootDirectory, $"{faqDocumentsDirectory}/{model.FaqSectionId}");
                newSectionFile.Type = FaqFileType.Image;
                newSectionFile.FaqSectionContentId = newSectionContent.Id;
                await _uow.FaqSectionContentFileRepository.AddAsync(newSectionFile);
            }
            if (model.Document != null){
                var newSectionFile = new FaqSectionContentFile();
                newSectionFile.FileUrl = await _fileService.SaveFileAsync(model.Document, rootDirectory, $"{faqDocumentsDirectory}/{model.FaqSectionId}");
                newSectionFile.Type = FaqFileType.File;
                newSectionFile.FaqSectionContentId = newSectionContent.Id;
                await _uow.FaqSectionContentFileRepository.AddAsync(newSectionFile);
            }

            if (model.Video != null)
            {
                var newSectionFile = new FaqSectionContentFile();
                newSectionFile.FileUrl = await _fileService.SaveFileAsync(model.Video, rootDirectory, $"{faqDocumentsDirectory}/{model.FaqSectionId}");
                newSectionFile.Type = FaqFileType.Video;
                newSectionFile.FaqSectionContentId = newSectionContent.Id;
                await _uow.FaqSectionContentFileRepository.AddAsync(newSectionFile);
            }
            await _uow.SaveAsync();

            return newSectionContent.FaqSectionId;
        }

        public async Task DeleteFaqAsync(Guid id)
        {
            await _uow.FaqRepository.DeleteAsync(id);
            await _uow.SaveAsync();
        }

        public async Task<Guid> DeleteFaqContentAsync(Guid id)
        {
            var faqContent = await _uow.FaqSectionContentRepository.GetByIdAsync(id);
            await _uow.FaqSectionContentRepository.DeleteAsync(id);
            await _uow.SaveAsync();
            return faqContent.FaqSectionId;
        }

        public async Task<Guid> DeleteFaqSectionAsync(Guid id)
        {
            var faqSection = await _uow.FaqSectionRepository.GetByIdAsync(id);
            await _uow.FaqSectionRepository.DeleteAsync(id);
            await _uow.SaveAsync();
            return faqSection.FaqId;
        }

        public List<FaqDTO> GetAllFaqs()
        {
            var faqs = _uow.FaqRepository.All.Include(x=>x.FaqSections);
            var mappedFaqs = _mapper.Map<List<FaqDTO>>(faqs);
            return mappedFaqs;
        }

        public FaqDTO GetFaqById(Guid faqId)
        {
            var faq = _uow.FaqRepository.GetFaqByIdIncluding(faqId);
            var dto = _mapper.Map<FaqDTO>(faq);
            return dto;
        }

        public async Task<string> GetFaqColorCode(Guid faqId)
        {
            var faq = await _uow.FaqRepository.GetByIdAsync(faqId);
            if (faq != null)
                return faq.ColorCode;
            return string.Empty;
        }

        public FaqSectionDto GetFaqSectionById(Guid id)
        {
            var sectionContent = _uow.FaqSectionRepository.GetFaqSectionByIdIncluding(id);
            var dto = _mapper.Map<FaqSectionDto>(sectionContent);
            return dto;
        }

        public async Task<List<FaqSectionContentDto>> GetFaqSectionContentAsync(Guid id)
        {
            var faqSectionContent = await _uow.FaqSectionContentRepository.GetFaqSectionContent(id);
            var mappedFaqContent = _mapper.Map<List<FaqSectionContentDto>>(faqSectionContent);
            return mappedFaqContent;
        }

        public async Task<FaqSectionContentDto> GetFaqSectionContentByIdAsync(Guid id)
        {
            var sectionContent = await _uow.FaqSectionContentRepository.GetByIdAsync(id);
            var sectionContentDto = _mapper.Map<FaqSectionContentDto>(sectionContent);
            return sectionContentDto;
        }

        public async Task<List<FaqSectionDto>> GetFaqSectionsAsync(Guid id)
        {
            var faqs = await _uow.FaqSectionRepository.GetFaqSections(id);
            var mappedFaqs = _mapper.Map<List<FaqSectionDto>>(faqs);
            return mappedFaqs;
        }

        public async Task UpdateFaqAsync(FaqDTO model)
        {
            var updatedFaq = new Faq() {Id = model.Id, Title = model.Title, ColorCode = model.ColorCode };
            _uow.FaqRepository.Update(updatedFaq);
            await _uow.SaveAsync();
        }

        public async Task<Guid> UpdateFaqSectionAsync(FaqSectionDto model, string faqDocumentsDirectory, string rootDirectory)
        {
            var faqSection = await _uow.FaqSectionRepository.GetByIdAsync(model.Id);

            if (faqSection == null)
                throw new Exception();
            faqSection.Title = model.Title;
            if (model.Icon != null)
                faqSection.IconUrl = await _fileService.SaveFileAsync(model.Icon, rootDirectory, $"{faqDocumentsDirectory}/{faqSection.Id}");
            
            _uow.FaqSectionRepository.Update(faqSection);
            await _uow.SaveAsync();
            return faqSection.FaqId;
        }

        public async Task<Guid> UpdateFaqSectionContentAsync(FaqSectionContentDto model, string faqDocumentsDirectory, string rootDirectory)
        {
            var updateFaqSectionContent = new FaqSectionContent() { Id = model.Id, Content = model.Content, FaqSectionId = model.FaqSectionId };
            if (model.Image != null){
                var storedSectionFile =
                    _uow.FaqSectionContentFileRepository.All.FirstOrDefault(z =>
                        z.FaqSectionContentId == updateFaqSectionContent.Id && z.Type==FaqFileType.Image);
                if (storedSectionFile == null){
                    var newFile = new FaqSectionContentFile() { Type = FaqFileType.Image, FaqSectionContentId = updateFaqSectionContent.Id };
                    newFile.FileUrl = await _fileService.SaveFileAsync(model.Image, rootDirectory, $"{faqDocumentsDirectory}/{model.FaqSectionId}");
                    await _uow.FaqSectionContentFileRepository.AddAsync(newFile);
                }else{
                    storedSectionFile.FileUrl = await _fileService.SaveFileAsync(model.Image, rootDirectory, $"{faqDocumentsDirectory}/{model.FaqSectionId}");
                    _uow.FaqSectionContentFileRepository.Update(storedSectionFile);
                }
            }
            if (model.Document != null){
                var storedSectionFile =
                    _uow.FaqSectionContentFileRepository.All.FirstOrDefault(z =>
                        z.FaqSectionContentId == updateFaqSectionContent.Id && z.Type == FaqFileType.File);
                if (storedSectionFile == null){
                    var newFile = new FaqSectionContentFile() { Type = FaqFileType.File, FaqSectionContentId = updateFaqSectionContent.Id };
                    newFile.FileUrl = await _fileService.SaveFileAsync(model.Document, rootDirectory, $"{faqDocumentsDirectory}/{model.FaqSectionId}");
                    await _uow.FaqSectionContentFileRepository.AddAsync(newFile);
                }else{
                    storedSectionFile.FileUrl = await _fileService.SaveFileAsync(model.Document, rootDirectory, $"{faqDocumentsDirectory}/{model.FaqSectionId}");
                    _uow.FaqSectionContentFileRepository.Update(storedSectionFile);
                }
            }

            if (model.Video != null){
                var storedSectionFile =
                    _uow.FaqSectionContentFileRepository.All.FirstOrDefault(z =>
                        z.FaqSectionContentId == updateFaqSectionContent.Id && z.Type == FaqFileType.Video);
                if (storedSectionFile == null){
                    var newFile = new FaqSectionContentFile() { Type = FaqFileType.Video, FaqSectionContentId = updateFaqSectionContent.Id };
                    newFile.FileUrl = await _fileService.SaveFileAsync(model.Video, rootDirectory, $"{faqDocumentsDirectory}/{model.FaqSectionId}");
                    await _uow.FaqSectionContentFileRepository.AddAsync(newFile);
                }else{
                    storedSectionFile.FileUrl = await _fileService.SaveFileAsync(model.Video, rootDirectory, $"{faqDocumentsDirectory}/{model.FaqSectionId}");
                    _uow.FaqSectionContentFileRepository.Update(storedSectionFile);
                }
            }
            _uow.FaqSectionContentRepository.Update(updateFaqSectionContent);
            await _uow.SaveAsync();
            return updateFaqSectionContent.FaqSectionId;
        }
    }
}   
