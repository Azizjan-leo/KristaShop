using System;
using KristaShop.Common.Interfaces.Models;
using Module.Media.Business.DTOs.Mappings;

namespace Module.Media.Business.DTOs {
    /// <summary>
    /// This DTO uses <see cref="VideoGalleryMappingProfile"/> mapping profile
    /// </summary>
    public class VideoGalleryDTO {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFileDataProvider Preview { get; set; }
        public string PreviewPath { get; set; }
        public string VideoPath { get; set; }
        public bool IsOpen { get; set; }
        public bool IsVisible { get; set; }
        public string Slug { get; set; }
        public int Order { get; set; }
        public int VideosCount { get; set; }
    }
}
