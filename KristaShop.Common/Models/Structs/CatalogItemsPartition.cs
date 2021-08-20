using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace KristaShop.Common.Models.Structs {
    [TypeConverter(typeof(CatalogItemsPartitionConverter))]
    public readonly struct CatalogItemsPartition {
        public List<Dictionary<string, bool>> Parts { get; }

        public CatalogItemsPartition(List<Dictionary<string, bool>> parts) {
            Parts = parts;
        }

        public bool IsEmpty() {
            return Parts == null || !Parts.Any();
        }
    }
    
    public class CatalogItemsPartitionConverter : TypeConverter {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
            if (sourceType == typeof(string)) {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context,
            CultureInfo culture, object value) {
            if (value is string valueString) {
                return _convertFromString(valueString);
            }

            return base.ConvertFrom(context, culture, value);
        }

        private CatalogItemsPartition _convertFromString(string value) {
            if (string.IsNullOrEmpty(value)) return default(CatalogItemsPartition);

            var result = new List<Dictionary<string, bool>>();

            var wrapParts = value.Split(new[] {';'});
            foreach (var wrapPart in wrapParts) {
                var group = new Dictionary<string, bool>();

                var items = wrapPart.Trim().Split(new[] {','});
                foreach (var item in items) {
                    var val = item.Trim();
                    if (!group.ContainsKey(val)) {
                        group.Add(val, true);
                    }
                }
                
                result.Add(group);
            }

            return new CatalogItemsPartition(result);
        }
    }
}