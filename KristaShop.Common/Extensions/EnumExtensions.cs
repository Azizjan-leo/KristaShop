using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using KristaShop.Common.Attributes;
using KristaShop.Common.Enums;

namespace KristaShop.Common.Extensions
{
    public static class EnumExtensions {
        public static string GetDisplayName(this Enum value) {
            var attribute = _getAttribute<DisplayAttribute>(value);
            return attribute == null ? string.Empty : attribute.Name;
        }
        
        public static string GetDisplayName(this State value) {
            var attribute = _getAttribute<DocumentStateAttribute>(value);
            return attribute == null ? string.Empty : attribute.Name;
        }
        
        public static string GetHighlightColor(this State value) {
            var attribute = _getAttribute<DocumentStateAttribute>(value);
            return attribute == null ? string.Empty : attribute.HighlightColor;
        }

        private static T _getAttribute<T>(Enum value) where T : Attribute {
            var fieldInfo = value.GetType().GetField(value.ToString());
            if (fieldInfo == null) return null;
            
            return (T)fieldInfo.GetCustomAttribute(typeof(T));
        }
    }
}