namespace KristaShop.Common.Models {
    public class GlobalSettings {
        public string RootLogin { get; set; }
        public string RootPassword { get; set; }
        public string FilesDirectoryPath { get; set; }
        public string DocumentFilesDirectory { get; set; }
        public string EditorFileStorageDirectory { get; set; }
        public string GalleryDirectory { get; set; }
        public string GalleryCache { get; set; }
        public string FeedbackFilesDirectory { get; set; }
        public string MenuContentDirectory { get; set; }
        public string VideoPreviewsDirectory { get; set; }
        public string VideoGalleryPreviewsDirectory { get; set; }
        public string CatalogPreviewsDirectory { get; set; }
        public string FaqDocumentsDirectory { get; set; }
        public string DatabaseBackupsDirectory { get; set; }
        public string ClientReportsFilesPath { get; set; }
        public string ClientInvoicesFilesPath { get; set; }
        public string PromoFilesDirectory { get; set; }
        public string DefaultManagerEmail { get; set; }
        public string GalleryPath => $"{FilesDirectoryPath}{GalleryDirectory}";
        public string DocumentFilesPath => $"{FilesDirectoryPath}{DocumentFilesDirectory}";
        public string EditorFileStoragePath => $"{FilesDirectoryPath}{EditorFileStorageDirectory}";
        public string FeedbackFilesPath => $"{FilesDirectoryPath}{FeedbackFilesDirectory}";
        public string DatabaseBackupsPath => $"{FilesDirectoryPath}{DatabaseBackupsDirectory}";
    }
}
