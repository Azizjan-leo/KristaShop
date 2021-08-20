using System.Collections.Generic;

namespace KristaShop.Common.Helpers {
    public class SizeStringComparer : IComparer<string> {
        public int Compare(string x, string y) {
            if (string.IsNullOrEmpty(y)) return 1;
            if (string.IsNullOrEmpty(x)) return -1;
            
            var dashIndex = x.IndexOf('-');
            if (dashIndex * y.IndexOf('-') > 0) {
                return x.CompareTo(y);
            }

            if (dashIndex > 0) {
                return 1;
            }

            return -1;
        }
    }
}