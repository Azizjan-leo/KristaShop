namespace Module.Common.WebUI.Models {
    public class NavbarViewModel {
        public int CartCount { get; set; }

        public NavbarViewModel(int cartCount) {
            CartCount = cartCount;
        }
    }
}