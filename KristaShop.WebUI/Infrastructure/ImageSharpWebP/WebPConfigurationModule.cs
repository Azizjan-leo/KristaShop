using Shorthand.ImageSharp.WebP;
using SixLabors.ImageSharp;

namespace KristaShop.WebUI.Infrastructure.ImageSharpWebP {
    public sealed class WebPConfigurationModule : IConfigurationModule
    {
        /// <inheritdoc/>
        public void Configure(Configuration configuration)
        {
            configuration.ImageFormatsManager.SetEncoder(WebPCustomFormat.Instance, new WebPEncoder { Quality = 75});
            // webp does not contain decoder at the time
            configuration.ImageFormatsManager.AddImageFormatDetector(new WebPImageFormatDetector());
        }
    }
}