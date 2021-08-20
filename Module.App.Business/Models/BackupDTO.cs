namespace Module.App.Business.Models {
    public class BackupDTO {
        public string TableName { get; set; }
        public int Order { get; set; }
        public string CreateStatement { get; set; }
        public string DataDumpFilePath { get; set; }
    }
}
