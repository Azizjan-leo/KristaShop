using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace KristaShop.Common.Attributes.AllowedFileExtensionAttribute {
    [AttributeUsage(AttributeTargets.Property)]
    public class AllowedFileExtensionsAttribute : ValidationAttribute {
        private const string imagesFilter = "image/*";

        private readonly List<string> _extensionsArray;

        public string Extensions { get; set; }

        public AllowedFileExtensionsAttribute(string extensions) {
            Extensions = extensions;

            _extensionsArray = extensions.Replace(" ", string.Empty).Split(",").ToList();
            if (_extensionsArray.Contains(imagesFilter)) {
                _extensionsArray.AddRange(_getImagesFilter());
                _extensionsArray.Add(".svg");
            }
        }

        public override bool IsValid(object value) {
            
            if (value is IFormFile file) {
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!_extensionsArray.Contains(extension)) {
                    return false;
                }
            }

            return true;
        }

        public override string FormatErrorMessage(string name) {
            if (string.IsNullOrEmpty(ErrorMessage)) {
                return $"File format of {name} is incorrect.";
            }

            return base.FormatErrorMessage(name);
        }

        private List<string> _getImagesFilter() {
            return new() {".bmp", ".dib", ".rle", ".jpg", ".jpeg", ".jpe", ".jfif", ".gif", ".tif", ".tiff", ".png", ".webp"};
        }
    }
}
