using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KristaShop.Common.Enums;
using KristaShop.Common.Interfaces.DataAccess;

namespace KristaShop.DataAccess.Entities.Media
{
    public class FaqSectionContentFile : IEntityKeyGeneratable
    {
        [Key]
        public Guid Id { get; set; }
        public string FileUrl { get; set; }
        public FaqFileType Type { get; set; }
        [ForeignKey("FaqSectionContent")]
        public Guid FaqSectionContentId { get; set; }
        public FaqSectionContent FaqSectionContent { get; set; }

        public FaqSectionContentFile() { }
        public FaqSectionContentFile(bool generateId)
        {
            if (generateId) GenerateKey();
        }

        public void GenerateKey()
        {
            Id = Guid.NewGuid();
        }
    }
}
