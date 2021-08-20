#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace KristaShop.DataAccess.Domain {
    public class DataChangeLogEntry {
        public int? UserId { get; }
        public EntityEntry Entry { get; }
        public string TableName { get; }
        public Dictionary<string, object> KeyValues { get; } = new();
        public Dictionary<string, object> OldValues { get; } = new();
        public Dictionary<string, object> NewValues { get; } = new();
        public List<PropertyEntry> TemporaryProperties { get; } = new();

        public bool HasTemporaryProperties => TemporaryProperties.Any();
        
        public DataChangeLogEntry(EntityEntry entry, string tableName, int? userId) {
            Entry = entry;
            TableName = tableName;
            UserId = userId;
        }

        public DataChangeLog ToLogEntity() {    
            return new() {
                TableName = TableName,
                EntityName = Entry.Entity.GetType().Name,
                Operation = Entry.State.ToString(),
                UserId = UserId,
                Timestamp = DateTimeOffset.UtcNow,
                KeyValues = System.Text.Json.JsonSerializer.Serialize(KeyValues),
                OldValues = OldValues.Count == 0 ? null : System.Text.Json.JsonSerializer.Serialize(OldValues),
                NewValues = NewValues.Count == 0 ? null : System.Text.Json.JsonSerializer.Serialize(NewValues)
            };
        }
    }
}