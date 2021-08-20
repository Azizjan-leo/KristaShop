using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Module.Media.Business.DTOs
{
    public class FaqSectionDto
    {
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string IconUrl { get; set; }
        public IFormFile Icon { get; set; }
        public Guid FaqId { get; set; }
        public ICollection<FaqSectionContentDto> FaqSectionContents { get; set; }
    }
}
