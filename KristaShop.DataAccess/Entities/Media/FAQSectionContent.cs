using System;
using System.Collections.Generic;
using KristaShop.Common.Interfaces.DataAccess;

namespace KristaShop.DataAccess.Entities.Media
{
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.FaqSectionContentConfiguration"/>
    /// </summary>

    public class FaqSectionContent : IEntityKeyGeneratable
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public Guid FaqSectionId { get; set; }
        public FaqSection FaqSection { get; set; }
        public ICollection<FaqSectionContentFile> FaqSectionContentFiles { get; set; }

        public FaqSectionContent() { }

        public FaqSectionContent(bool generateId)
        {
            if (generateId) GenerateKey();
        }

        public void GenerateKey()
        {
            Id = Guid.NewGuid();
        }
    }
}
