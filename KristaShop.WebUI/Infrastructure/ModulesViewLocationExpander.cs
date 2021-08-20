using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Razor;

namespace KristaShop.WebUI.Infrastructure {
    public class ModulesViewLocationExpander : IViewLocationExpander {
        public void PopulateValues(ViewLocationExpanderContext context) {
            
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations) {
            return viewLocations.Concat(new[] {
                "/{2}/Views/{1}/{0}" + RazorViewEngine.ViewExtension,
                "/{2}/Views/Shared/{0}" + RazorViewEngine.ViewExtension,
            });
        }
    }
}