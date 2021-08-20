using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace KristaShop.Common.Extensions {
    public static class DictionaryExtensions {
        public static ConcurrentDictionary<TKey, TElement> ToConcurrentDictionary<TKey, TValue, TElement>(
            this IEnumerable<TValue> source, Func<TValue, TKey> keySelector, Func<TValue, TElement> elementSelector) {
            return new ConcurrentDictionary<TKey, TElement>(
                from v in source
                select new KeyValuePair<TKey, TElement>(keySelector(v), elementSelector(v)));
        }
    }
}