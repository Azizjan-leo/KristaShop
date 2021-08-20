using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace KristaShop.Common.Attributes.RequiredThisOrOtherAttribute {
    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredThisOrOtherAttribute : ValidationAttribute {
        public string OtherProperty { get; set; }
        public string OtherPropertyDisplayName { get; set; }

        public RequiredThisOrOtherAttribute(string otherProperty) {
            OtherProperty = otherProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            PropertyInfo otherPropertyInfo = validationContext.ObjectType.GetProperty(OtherProperty);
            if (otherPropertyInfo == null) {
                return new ValidationResult($"Property {OtherProperty} not found");
            }

            var otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);
            if (!_isValid(value, otherPropertyValue)) {
                if (string.IsNullOrEmpty(OtherPropertyDisplayName)) {
                    OtherPropertyDisplayName = GetDisplayNameForProperty(validationContext.ObjectType, OtherProperty);
                }

                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            return ValidationResult.Success;
        }

        protected virtual bool _isValid(object value, object otherValue) {
            if (value == null && otherValue == null) {
                return false;
            }

            return true;
        }

        public override string FormatErrorMessage(string name) {
            if (string.IsNullOrEmpty(ErrorMessage)) {
                return $"Field {name} or {OtherPropertyDisplayName} is required.";
            }

            return FormatErrorMessage(name, OtherPropertyDisplayName);
        }

        public string FormatErrorMessage(string name, string otherName) {
            return string.Format(ErrorMessage, name, otherName);
        }

        public static string GetDisplayNameForProperty(Type containerType, string propertyName) {
            ICustomTypeDescriptor typeDescriptor = GetTypeDescriptor(containerType);
            PropertyDescriptor property = typeDescriptor.GetProperties().Find(propertyName, true);
            if (property == null) {
                throw new ArgumentException($"Property {propertyName} not found");
            }

            var attributes = property.Attributes.Cast<Attribute>().ToList();
            DisplayAttribute display = attributes.OfType<DisplayAttribute>().FirstOrDefault();
            if (display != null) {
                return display.GetName();
            }

            DisplayNameAttribute displayName = attributes.OfType<DisplayNameAttribute>().FirstOrDefault();
            if (displayName != null) {
                return displayName.DisplayName;
            }

            return propertyName;
        }

        private static ICustomTypeDescriptor GetTypeDescriptor(Type type) {
            return new AssociatedMetadataTypeTypeDescriptionProvider(type).GetTypeDescriptor(type);
        }
    }
}