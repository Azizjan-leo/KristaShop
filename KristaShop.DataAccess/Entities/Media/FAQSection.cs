using System;
using System.Collections.Generic;
using KristaShop.Common.Interfaces.DataAccess;

namespace KristaShop.DataAccess.Entities.Media
{
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.FaqSectionConfiguration"/>
    /// </summary>
    public class FaqSection : IEntityKeyGeneratable
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid FaqId { get; set; }
        public string IconUrl { get; set; }
        public Faq Faq { get; set; }
        public IEnumerable<FaqSectionContent> FaqSectionContents { get; set; }

        public FaqSection() { }

        public FaqSection(bool generateId)
        {
            if (generateId) GenerateKey();
        }

        public void GenerateKey()
        {
            Id = Guid.NewGuid();
        }
    }
}
