using System.Linq;
using KristaShop.Common.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Module.Common.WebUI.Extensions {
    public static class ModelStateExtensions {
        public static OperationResult ErrorsAsOperationResult(this ModelStateDictionary.ValueEnumerable values) {
            var errors = values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            return OperationResult.Failure(errors.ToList());
        }

        public static bool TryAddModelErrors(this ModelStateDictionary modelStateDictionary, string[] errors) {
            var result = true;
            foreach (var error in errors) {
                if (!modelStateDictionary.TryAddModelError(string.Empty, error)) {
                    result = false;
                    break;
                }
            }

            return result;
        }
    }
}
