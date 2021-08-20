using System;
using System.Globalization;

namespace KristaShop.Common.Extensions {
    public static class DateTimeExtension {
        public const string BasicFormatter = "dd.MM.yyyy";
        public const string SystemFormatter = "yyyy-MM-dd HH:mm";
        private const string FormattedFormatter = "dd.MM.yyyy HH:mm";
        private const string FullMonthFormatter = "dd MMMM yyyy";
        private const string ComputedFormatter = "MM/dd/yyyy HH:mm:ss";
        private const string JointFormatter = "ddMMyyyy";

        public static string ToSystemString(this DateTime dateTime) {
            return dateTime.ToString(SystemFormatter);
        }

        public static string ToSystemString(this DateTime? dateTime) {
            return dateTime != null ? ((DateTime)dateTime).ToString(SystemFormatter) : "";
        }

        public static string ToFormattedString(this DateTime dateTime) {
            return dateTime.ToString(FormattedFormatter);
        }

        public static string ToFormattedString(this DateTime? dateTime) {
            return dateTime != null ? ((DateTime)dateTime).ToString(FormattedFormatter) : "";
        }

        public static string ToBasicString(this DateTime dateTime) {
            return dateTime.ToString(BasicFormatter);
        }

        public static string ToBasicString(this DateTime? dateTime) {
            return dateTime != null ? ((DateTime)dateTime).ToBasicString() : "";
        }

        public static string ToJointString(this DateTime dateTime) {
            return dateTime.ToString(JointFormatter);
        }

        public static string ToJointString(this DateTime? dateTime) {
            return dateTime != null ? ((DateTime)dateTime).ToJointString() : "";
        }
        
        public static string ToFullMonthString(this DateTime dateTime) {
            return dateTime.ToString(FullMonthFormatter, CultureInfo.CreateSpecificCulture("ru"));
        }

        public static string ToFullMonthString(this DateTime? dateTime) {
            return dateTime != null ? ((DateTime)dateTime).ToFullMonthString() : "";
        }

        public static string ToComputedString(this DateTimeOffset dateTime) {
            return dateTime.ToString(ComputedFormatter, CultureInfo.CreateSpecificCulture("eu"));
        }

        public static string ToComputedString(this DateTimeOffset? dateTime) {
            return dateTime != null ? ((DateTimeOffset)dateTime).ToString(ComputedFormatter, CultureInfo.CreateSpecificCulture("eu")) : "";
        }

        public static string ToFormattedString(this DateTimeOffset dateTime) {
            return dateTime.ToString(FormattedFormatter);
        }

        public static string ToFormattedString(this DateTimeOffset? dateTime) {
            return dateTime != null ? ((DateTimeOffset)dateTime).ToString(FormattedFormatter) : "";
        }
        
        public static string ToBasicString(this DateTimeOffset dateTime) {
            return dateTime.ToString(BasicFormatter, CultureInfo.CreateSpecificCulture("eu"));
        }

        public static string ToBasicString(this DateTimeOffset? dateTime) {
            return dateTime != null ? ((DateTimeOffset)dateTime).ToBasicString() : "";
        }
        
        public static DateTime TryParseSystemString(string value) {
            return DateTime.TryParseExact(value, SystemFormatter, CultureInfo.CurrentCulture, DateTimeStyles.None, out var result) ? result : DateTime.MinValue;
        }
        
        public static DateTime TryParseBasicString(string value) {
            return DateTime.TryParseExact(value, BasicFormatter, CultureInfo.CurrentCulture, DateTimeStyles.None, out var result) ? result : DateTime.MinValue;
        }
    }
}