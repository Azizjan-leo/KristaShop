using System;
using System.Collections.Generic;
using KristaShop.Common.Interfaces.DataAccess;

namespace KristaShop.DataAccess.Entities.Media
{
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.FaqConfiguration"/>
    /// </summary>
    public class Faq : IEntityKeyGeneratable
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
        public string ColorCode { get; set; }
        public ICollection<FaqSection> FaqSections { get; set; }
        public Faq() { }

        public Faq(bool generateId)
        {
            if (generateId) GenerateKey();
        }
        public void GenerateKey()
        {
            Id = Guid.NewGuid();
        }
    }
}
