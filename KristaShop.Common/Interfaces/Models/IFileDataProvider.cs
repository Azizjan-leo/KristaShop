using System.IO;

namespace KristaShop.Common.Interfaces.Models {
    public interface IFileDataProvider {
        string OriginalName { get; set; }
        Stream FileStream { get; set; }
        string FilesDirectoryPath { get; set; }
        string Directory { get; set; }
    }
}
