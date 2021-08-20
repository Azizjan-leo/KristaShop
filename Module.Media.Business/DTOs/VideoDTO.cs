using System;
using System.Collections.Generic;
using KristaShop.Common.Interfaces.Models;
using Module.Media.Business.DTOs.Mappings;

namespace Module.Media.Business.DTOs {
    /// <summary>
    /// This DTO uses <see cref="VideoGalleryMappingProfile"/>
    /// </summary>
    public class VideoDTO {
        public Guid Id { get; set; }
        public string PreviewPath { get; set; }
        public IFileDataProvider Preview { get; set; }
        public string VideoPath { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsVisible { get; set; }
        public int Order { get; set; }
        public List<Guid> GalleryIds { get; set; }
    }
}
