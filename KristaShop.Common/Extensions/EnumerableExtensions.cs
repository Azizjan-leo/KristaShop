using System;
using System.Collections.Generic;
using System.Linq;
using KristaShop.Common.Models.Structs;
using P.Pager;

namespace KristaShop.Common.Extensions {
    public static class EnumerableExtensions {
        public static IEnumerable<TResult> SortBy<TResult, TKey>(this IEnumerable<TResult> sortItems,
            IEnumerable<TKey> sortKeys, Func<TResult, TKey> matchFunc) {
            return sortKeys.Join(sortItems, k => k, matchFunc, (k, i) => i);
        }

        public static IPager<TResult> ToPagerList<TResult>(this IEnumerable<TResult> items, Page page, int totalItemsCount) {
            var result = items.ToPagerList(page.Index, page.Size);
            result.TotalItemCount = totalItemsCount;
            result.CurrentPageIndex = page.Index + 1;
            return result;
        }
    }
}