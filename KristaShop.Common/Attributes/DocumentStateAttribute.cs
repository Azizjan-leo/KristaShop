using System;

namespace KristaShop.Common.Attributes {
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class DocumentStateAttribute : Attribute {
        public string Name { get; set; }
        public string HighlightColor { get; set; }
    }
}