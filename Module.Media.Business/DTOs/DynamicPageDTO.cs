using System;
using KristaShop.Common.Helpers;
using KristaShop.Common.Interfaces.Models;

namespace Module.Media.Business.DTOs {
    public class DynamicPageDTO {
        public Guid Id { get; set; }
        public string URL { get; set; }
        public string Title { get; set; }
        public string TitleIconPath { get; set; }
        public IFileDataProvider TitleIcon { get; set; }
        public string ImagePath { get; set; }
        public IFileDataProvider Image { get; set; }
        public string Body { get; set; }
        public string Layout { get; set; }
        public bool IsOpen { get; set; }
        public bool IsSinglePage { get; set; }
        public bool IsVisibleInMenu { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public int Order { get; set; }
        

        public string GetActionName() {
             return UrlHelper.DeconstructUri(URL).Item2;
        }
    }
}