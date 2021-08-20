using System;
using KristaShop.Common.Enums;
using KristaShop.Common.Interfaces.Models;

namespace Module.App.Business.Models {
    public class PromoLinkDTO {
        public Guid Id { get; set; }
        public string Link { get; set; }
        public int ManagerId { get; set; }
        public OrderFormType OrderForm { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset DeactivateTime { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string VideoPath { get; set; }
        public string VideoPreviewPath { get; set; }
        public IFileDataProvider VideoPreview { get; set; }
    }
}
