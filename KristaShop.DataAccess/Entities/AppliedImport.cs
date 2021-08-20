using System;
using KristaShop.DataAccess.Configurations;

namespace KristaShop.DataAccess.Entities {
    /// <summary>
    /// Configuration file for this entity <see cref="AppliedImportConfiguration"/>
    /// </summary>
    public class AppliedImport{
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string KeyValue { get; set; }
        public DateTimeOffset ApplyDate { get; set; }
        public string BackupFile { get; set; }
        public int UserId { get; set; }

        public AppliedImport() { }

        public AppliedImport(string key, string keyValue, string backupFile, int userId) {
            Id = Guid.NewGuid();
            Key = key;
            KeyValue = keyValue;
            BackupFile = backupFile;
            UserId = userId;
            ApplyDate = DateTimeOffset.UtcNow;
        }
    }
}
