using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Module.Media.Business.DTOs
{
    public class FaqSectionContentDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public bool IsDocument { get; set; }
        public bool IsImage { get; set; }
        public Guid FaqSectionId { get; set; }
        public IFormFile Document { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile Image { get; set; }
        public string DocumentUrl { get; set; }
        public IFormFile Video { get; set; }
        public string VideoUrl { get; set; }
        public ICollection<FaqSectionContentFileDto> FaqSectionContentFileDtos { get; set; }

    }
}
