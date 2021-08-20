namespace Module.Common.WebUI.Models {
    public class MetaViewModel {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }

        public MetaViewModel(string title = "", string description = "", string keywords = "") {
            Title = title;
            Description = description;
            Keywords = keywords;
        }
    }
}
