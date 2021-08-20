using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Module.Common.WebUI.Base;

namespace Module.Common.WebUI.ViewComponents {
    public class Success : ViewComponentBase {
        public async Task<IViewComponentResult> InvokeAsync(string header, string message) {
            return View((header, message));
        }
    }
}