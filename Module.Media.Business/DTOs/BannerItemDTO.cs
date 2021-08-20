using System;
using KristaShop.Common.Interfaces.Models;

namespace Module.Media.Business.DTOs {
    public class BannerItemDTO {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string TitleColor { get; set; }
        public string Caption { get; set; }
        public IFileDataProvider Image { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public bool IsVisible { get; set; }
        public int Order { get; set; }
    }
}