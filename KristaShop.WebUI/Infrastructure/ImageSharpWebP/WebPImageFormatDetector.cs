using System;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;

namespace KristaShop.WebUI.Infrastructure.ImageSharpWebP {
    /// <summary>
    /// Detects gif file headers
    /// </summary>
    public sealed class WebPImageFormatDetector : IImageFormatDetector
    {
        /// <inheritdoc/>
        public int HeaderSize => 12;

        /// <inheritdoc/>
        public IImageFormat DetectFormat(ReadOnlySpan<byte> header)
        {
            return this.IsSupportedFileFormat(header) ? WebPCustomFormat.Instance : null;
        }

        private bool IsSupportedFileFormat(ReadOnlySpan<byte> header)
        {
            // RIFF WEBP
            // webp magic number in ascii hex: 52 49 46 46 xx xx xx xx 57 45 42 50
            return header.Length >= this.HeaderSize &&
                   header[0] == 0x52 && // R
                   header[1] == 0x49 && // I
                   header[2] == 0x46 && // F
                   header[3] == 0x46 && // F
                   header[8] == 0x57 && // W
                   header[9] == 0x45 && // E
                   header[10] == 0x42 && // B
                   header[11] == 0x50; // P
        }
    }
}