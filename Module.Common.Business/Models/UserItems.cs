using System.Collections.Generic;

namespace Module.Common.Business.Models {
    public class UserItems<T> where T : class {
        public int UserId { get; set; }
        public IEnumerable<T> Items { get; set; }

        public UserItems(int userId, IEnumerable<T> items) {
            UserId = userId;
            Items = items;
        }
    }
}