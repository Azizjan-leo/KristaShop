using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Module.Media.Business.DTOs
{
    public class FaqDTO
    {
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string ColorCode { get; set; }
        public IEnumerable<FaqSectionDto> FaqSections { get; set; }
    }

    public class FaqViewModel 
    {
        public List<FaqDTO> Faqs { get; set; }
        public List<FaqSectionDto> SelectedFaqSections { get; set; }
        public string SelectedSectionColorCode { get; set; }
    }
}
