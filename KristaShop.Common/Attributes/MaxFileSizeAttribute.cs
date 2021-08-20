using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace KristaShop.Common.Attributes {

    [AttributeUsage(AttributeTargets.Property)]
    public class MaxFileSizeAttribute : ValidationAttribute {
        private readonly int _maxFileSize;
        public MaxFileSizeAttribute(int maxFileSize) {
            _maxFileSize = maxFileSize;
        }

        public override bool IsValid(object value) {
            if (value is IFormFile file) {
                if (file.Length > _maxFileSize) {
                    return false;
                }
            }

            return true;
        }

        public override string FormatErrorMessage(string name) {
            if (string.IsNullOrEmpty(ErrorMessage)) {
                return $"Maximum allowed file size of {name} is { _maxFileSize} bytes.";
            }

            return base.FormatErrorMessage(name);
        }
    }
}
