using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Cyrillic.Convert;

namespace KristaShop.Common.Extensions {
    public static class StringExtensions {
        public static string ToValidFileName(this string name) {
            var result = name.ToRussianLatin();
            var regex = new Regex(@"[^a-zA-Z0-9\\.\\_-]");
            result = regex.Replace(result, "");
            return result;
        }

        public static string ToValidLogin(this string name) {
            var result = name.ToRussianLatin();
            var regex = new Regex(@"[^a-zA-Z0-9]");
            result = regex.Replace(result, "");
            return result;
        }

        public static string ToValidLatinString(this string name) {
            var result = name.ToRussianLatin().Trim();
            var spacesRegex = new Regex(@"[ ]+");
            var regex = new Regex(@"[^a-zA-Z0-9\\.\\_-]");
            result = spacesRegex.Replace(result, "-");
            result = regex.Replace(result, "");
            return result;
        }

        public static string InvertPathSeparator(this string path) {
            return path.Replace("\\\\", "/").Replace("\\", "/");
        }

        public static bool IsSizesEqual(this string value1, string value2) {
            var parts1 = value1.Split(new char[] { '_' });
            var parts2 = value2.Split(new char[] { '_' });
            if (parts1[0] != parts2[0]) return false;

            if (parts1[0] == "S") {
                if (parts1.Length != parts2.Length || parts1.Length < 4) return false;

                if (parts1[1] == parts2[1] && parts1[3] == parts2[3]) {
                    return true;
                } else {
                    return false;
                }
            } else {
                return value1 == value2;
            }
        }

        public static bool IsSizeKeyForLine(this string value) {
            var parts = value.Split(new char[] { '_' });
            return parts[0] == "L";
        }

        public static DateTime? ToUserCreateDate(this string value) {
            try {
                value = value.Trim();

                if (string.IsNullOrEmpty(value)) {
                    return null;
                } else if (!Regex.IsMatch(value, @"^[0-9]{4,4}-[0-9]{2,2}-[0-9]{2,2} [0-9]{2,2}:[0-9]{2,2}$", RegexOptions.IgnoreCase)) {
                    return null;
                } else if (value == "0000-00-00 00:00") {
                    return null;
                } else {
                    return DateTime.ParseExact(value, "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                }
            } catch (Exception ex) {
                if (ex != null) { 

                }
                return null;
            }
        }

        public static FormattableString ToFormattable(this string value) {
            return FormattableStringFactory.Create(value);
        }
        
        // Generate same has code for same string value on each application rub
        public static int GetSameHashCode(this string value) {
            var md5Hasher = MD5.Create();
            var hashed = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(value));
            return BitConverter.ToInt32(hashed, 0);
        }

        private static Dictionary<string, string> moneyDocumensNames = new Dictionary<string, string>() {
            {"act", "Акт"},
            {"zayavka", "Заявка"},
            {"schet", "Счет"},
            {"torg12", "Торг 12"}
        };
        
        public static string GetMoneyFileName(string fileName) {
            var index = fileName.IndexOf("_", StringComparison.Ordinal);
            if (index > 0)
                fileName = fileName[..index];
            return moneyDocumensNames.ContainsKey(fileName) ? moneyDocumensNames[fileName] : "";
        }
    }
}
