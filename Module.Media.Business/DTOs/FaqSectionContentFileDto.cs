using System;
using KristaShop.Common.Enums;

namespace Module.Media.Business.DTOs
{
    public class FaqSectionContentFileDto
    {
        public Guid Id { get; set; }
        public string FileUrl { get; set; }
        public FaqFileType Type { get; set; }
    }
}
