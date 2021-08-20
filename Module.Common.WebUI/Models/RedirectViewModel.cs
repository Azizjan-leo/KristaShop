using KristaShop.Common.Helpers;
using KristaShop.Common.Models;

namespace Module.Common.WebUI.Models {
    public class RedirectViewModel {
        public string Location { get; set; }
        public OperationResult OperationResult { get; set; }

        public RedirectViewModel(string action, string controller) {
            Location = UrlHelper.GetURL(controller, action);
        }

        public RedirectViewModel(string action, string controller, OperationResult operationResult) {
            Location = UrlHelper.GetURL(controller, action);
            OperationResult = operationResult;
        }
    }
}
