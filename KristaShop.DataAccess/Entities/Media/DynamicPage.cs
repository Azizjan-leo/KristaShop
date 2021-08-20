using System;
using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.DataAccess.Configurations;

namespace KristaShop.DataAccess.Entities.Media {
    /// <summary>
    /// Configuration file for this entity <see cref="DynamicPageConfiguration"/>
    /// </summary>
    public class DynamicPage : IEntityKeyGeneratable {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string TitleIconPath { get; set; }
        public string ImagePath { get; set; }
        public string Body { get; set; }
        public string Layout { get; set; }
        public bool IsOpen { get; set; }
        public bool IsSinglePage { get; set; }
        public bool IsVisibleInMenu { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public int Order { get; set; } = 0;

        public void GenerateKey() {
            Id = Guid.NewGuid();
        }
    }
}