using System;
using Microsoft.AspNetCore.Http;

namespace Module.Media.Business.DTOs
{
    public class BlogItemDTO
    {
        public Guid Id { get; set; }
        public IFormFile Image { get; set; }
        public string ImagePath { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string LinkText { get; set; }
        public string Link { get; set; }
        public bool IsVisible { get; set; }
        public int Order { get; set; }
    }
}