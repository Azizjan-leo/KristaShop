using System;

namespace KristaShop.Common.Extensions {
    public static class GuidExtensions {
        public static bool IsEmpty(this Guid guid) {
            return guid == Guid.Empty;
        }
    }
}
