using System;
using System.Collections.Generic;
using System.Linq;
using KristaShop.Common.Extensions;

namespace KristaShop.Common.Models {
    public class LookUpItem<TKey, TValue> {
        public TKey Key { get; set; }
        public TValue Value { get; set; }

        public LookUpItem(TKey key, TValue value) {
            Key = key;
            Value = value;
        }
    }

    public static class LookUpItem {
        public static IEnumerable<LookUpItem<string, string>> FromEnum<T>() where T : struct, Enum {
            return Enum.GetValues<T>().Select(x => new LookUpItem<string, string>(x.ToString(), x.GetDisplayName())).ToList();
        }
    }
}
