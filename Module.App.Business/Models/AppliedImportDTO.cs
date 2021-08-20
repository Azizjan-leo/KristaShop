using System;
using System.IO;
using KristaShop.Common.Extensions;

namespace Module.App.Business.Models {
    public class AppliedImportDTO {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string KeyValue { get; set; }
        public DateTimeOffset ApplyDate { get; set; }
        public string BackupFile { get; set; }
        public int UserId { get; set; }
        public string Login { get; set; }

        public string BackupFileName => Path.GetFileNameWithoutExtension(BackupFile);
        public string ApplyDateFormatted => ApplyDate.ToFormattedString();
    }
}
