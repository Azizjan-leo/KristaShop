using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace KristaShop.Common.Attributes.FileRequired {

    [AttributeUsage(AttributeTargets.Property)]
    public class FileRequiredAttribute : ValidationAttribute {
        public override bool IsValid(object value) {
            return value is IFormFile;
        }

        public override string FormatErrorMessage(string name) {
            if (string.IsNullOrEmpty(ErrorMessage)) {
                return $"{name} is required.";
            }

            return base.FormatErrorMessage(name);
        }
    }
}
