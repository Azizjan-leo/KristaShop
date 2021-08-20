using System.IO;
using KristaShop.Common.Interfaces.Models;

namespace Module.App.Business.Models {
    public class FeedbackCreateFileDTO : IFileDataProvider {
        public string OriginalName { get; set; }
        public Stream FileStream { get; set; }
        public string FilesDirectoryPath { get; set; }
        public string Directory { get; set; }
    }
}
