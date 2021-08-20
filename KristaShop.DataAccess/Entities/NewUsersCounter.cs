using System;
using KristaShop.Common.Interfaces.DataAccess;

namespace KristaShop.DataAccess.Entities {
    public class NewUsersCounter : IEntityKeyGeneratable {
        public Guid Id { get; set; }
        public long Counter { get; set; }
        public DateTimeOffset? UpdateTimestamp { get; set; }

        public NewUsersCounter() { }

        public NewUsersCounter(long counter) {
            Counter = counter;
            SetUpdateTimestamp();
        }

        public void SetUpdateTimestamp() {
            UpdateTimestamp = DateTimeOffset.UtcNow;
        }

        public void IncreaseCounter() {
            Counter++;
            SetUpdateTimestamp();
        }
        public void ResetCounter() {
            Counter = 1;
            SetUpdateTimestamp();
        }

        public void GenerateKey() {
            Id = Guid.NewGuid();
        }
    }
}