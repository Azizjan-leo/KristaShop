using Shorthand.ImageSharp.WebP;

namespace KristaShop.WebUI.Infrastructure.ImageSharpWebP {
    public class WebPCustomFormat : WebPFormat {
        public static WebPCustomFormat Instance { get; } = new WebPCustomFormat();
    }
}