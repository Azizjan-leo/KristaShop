using System;

namespace KristaShop.DataAccess.Views {
    /// <summary>
    /// Configuration file for this entity <see cref="Configurations.Views.AppliedImportItemConfiguration"/>
    /// </summary>

    public class AppliedImportItem {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string KeyValue { get; set; }
        public DateTimeOffset ApplyDate { get; set; }
        public string BackupFile { get; set; }
        public int UserId { get; set; }
        public string Login { get; set; }
    }
}
