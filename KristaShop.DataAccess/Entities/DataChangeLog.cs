using System;
using KristaShop.DataAccess.Configurations;

namespace KristaShop.DataAccess.Entities {
    /// <summary>
    /// Configuration file for this entity <see cref="DataChangeLogConfiguration"/>
    /// </summary>
    public class DataChangeLog {
        public Guid Id { get; set; }
        public string TableName { get; set; }
        public string EntityName { get; set; }
        public string Operation { get; set; }
        public int? UserId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string KeyValues { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
    }
}