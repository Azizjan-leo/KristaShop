using System.IO;
using KristaShop.Common.Extensions;
using KristaShop.Common.Interfaces.Models;

namespace KristaShop.Common.Models {
    public class FileDataProvider : IFileDataProvider {
        private string _originalName = string.Empty;

        public string OriginalName {
            get => _originalName;
            set => _originalName = value.ToValidFileName();
        }

        public Stream FileStream { get; set; }
        public string FilesDirectoryPath { get; set; }
        public string Directory { get; set; }
    }
}
