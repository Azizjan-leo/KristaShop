namespace Module.Common.WebUI.Models {
    public class TitleDescriptionViewModel {
        public string Title { get; set; }
        public string ComponentName { get; }
        public object ComponentParams { get; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string HideButtonText { get; set; }
        public string ShowButtonText { get; set; }
        public bool IsCollapsed { get; set; }
        public string GetDefaultText => IsCollapsed ? ShowButtonText : HideButtonText;
        public bool IsHeading { get; set; }

        public TitleDescriptionViewModel(string title, string description, string link = "",
            string hideButtonText = "Скрыть описание", string showButtonText = "Показать описание",
            bool isCollapsed = false, bool isHeading = false) {
            Title = title;
            Description = description;
            Link = link;
            HideButtonText = hideButtonText;
            ShowButtonText = showButtonText;
            IsCollapsed = isCollapsed;
            IsHeading = isHeading;
        }
        
        public TitleDescriptionViewModel(string title, string componentName, object componentParams, string link = "",
            string hideButtonText = "Скрыть описание", string showButtonText = "Показать описание",
            bool isCollapsed = false, bool isHeading = false) {
            Title = title;
            ComponentName = componentName;
            ComponentParams = componentParams;
            Description = "";
            Link = link;
            HideButtonText = hideButtonText;
            ShowButtonText = showButtonText;
            IsCollapsed = isCollapsed;
            IsHeading = isHeading;
        }
    }
}