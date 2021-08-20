using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using KristaShop.Common.Extensions;

namespace KristaShop.Common.Models.Structs {
    [TypeConverter(typeof(DateRangeConverter))]
    public struct DateRange {
        public DateTimeOffset? From { get; set; }
        public DateTimeOffset? To { get; set; }

        public DateRange(DateTimeOffset? from, DateTimeOffset? to) {
            From = from;
            To = to;
        }

        public static bool TryParse(string range, out DateRange result) {
            result = new DateRange(null, null);
            try {
                if (!string.IsNullOrEmpty(range)) {
                    var dates = range.Split(" - ");
                    if (!dates.Any()) return false;
                    
                    var from = DateTimeOffset.Parse(dates[0]);
                    var to = dates.Length == 2 ? DateTimeOffset.Parse(dates[1]).AddDays(1).AddMilliseconds(-1) : from.AddDays(1).AddMilliseconds(-1);
                    result = new DateRange(from, to);
                }
                return true;
            } catch (Exception) {
                return false;
            }
        }

        public static DateRange FromLastMonth() {
            return new(new DateTimeOffset(DateTime.Now.Year, DateTime.Now.Month - 1, 1, 0, 0, 0, TimeSpan.Zero),
                DateTimeOffset.Now);
        }
        
        public override string ToString() {
            if (!From.HasValue || !To.HasValue) {
                return "";
            }
            
            return $"{From.ToBasicString()} - {To.ToBasicString()}";
        }
    }

    public class DateRangeConverter : TypeConverter {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
            if (sourceType == typeof(string)) {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context,
            CultureInfo culture, object value) {
            if (value is string valueString) {
                if (DateRange.TryParse(valueString, out var range)) {
                    return range;
                }
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}