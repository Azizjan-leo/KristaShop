using System;
using System.Collections.Generic;
using KristaShop.Common.Enums;

namespace Module.App.Business.Models {
    public class MenuItemDTO {
        public Guid Id { get; set; }
        public MenuType MenuType { get; set; }
        public string Title { get; set; }
        public string AreaName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string Icon { get; set; }
        public int Order { get; set; }
        public string BadgeTarget { get; set; }
        public Guid? ParentId { get; set; }
        public string Url { get; set; }
        public List<string> ChildControllers { get; set; }
    }
}