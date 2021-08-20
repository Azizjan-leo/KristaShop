using System.Collections.Generic;

namespace Module.Common.Admin.Admin.Models {
    public class MenuButtonsViewModel {
        public string Title { get; set; }
        public string IconName { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public Dictionary<string, string> RouteValues { get; set; }
        public string Classes { get; set; }
        public string OnClickJSCode { get; set; }

        public MenuButtonsViewModel(string title, string action, string controller = "", string iconName = "", Dictionary<string, string> routeValues = null, string classes = "", string onClickJSCode = "") {
            Title = title;
            Action = action;
            Controller = controller;
            IconName = iconName;
            RouteValues = routeValues;
            Classes = classes;
            OnClickJSCode = onClickJSCode;
        }
    }
}
