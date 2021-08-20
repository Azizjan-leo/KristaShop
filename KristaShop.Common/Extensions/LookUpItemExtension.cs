using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KristaShop.Common.Extensions {
    public static class LookUpItemExtension {
        public static SelectList AsSelectList<TKey, TValue>(this IEnumerable<LookUpItem<TKey, TValue>> lookup) {
            return new SelectList(lookup, "Key", "Value");
        }
        
        public static async Task<SelectList> AsSelectListAsync<TKey, TValue>(this Task<List<LookUpItem<TKey, TValue>>> lookupTask) {
            return new SelectList(await lookupTask, "Key", "Value");
        }
        
        public static async Task<SelectList> AsSelectListAsync<TKey, TValue>(this Task<IReadOnlyList<LookUpItem<TKey, TValue>>> lookupTask) {
            return new SelectList(await lookupTask, "Key", "Value");
        }
    }
}