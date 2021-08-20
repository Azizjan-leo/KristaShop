using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Module.Media.Business.Interfaces;

namespace Module.Media.WebUI.Infrastructure {
    public class DynamicPagesRouteValueTransformer : DynamicRouteValueTransformer {
        private readonly IDynamicPagesManager _manager;

        public DynamicPagesRouteValueTransformer(IDynamicPagesManager manager) {
            _manager = manager;
        }

        public override async ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values) {
            var metaInfoPath = KristaShop.Common.Helpers.UrlHelper.GetURL(values["controller"].ToString(), values["action"].ToString());
            
            if (_manager.TryGetValue(metaInfoPath, out var content)) {
                values["controller"] = "DynamicPages";
                values["action"] = "Index";
                values["uri"] = content.URL;

                if (content.IsSinglePage) {
                    values["action"] = "Single";
                }
            }

            return values;
        }
    }
}
