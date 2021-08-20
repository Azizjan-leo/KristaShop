using System;
using KristaShop.Common.Enums;

namespace KristaShop.DataAccess.Entities {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.PromoLinkConfiguration"/>
    /// </summary>
    public class PromoLink {
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
    }
}