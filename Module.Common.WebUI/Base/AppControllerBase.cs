using System;
using System.Text.Json;
using System.Threading.Tasks;
using KristaShop.Common.Extensions;
using KristaShop.Common.Interfaces;
using KristaShop.Common.Models;
using KristaShop.Common.Models.Session;
using KristaShop.Common.Models.Structs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Module.Common.Business.Interfaces;
using Module.Common.WebUI.Extensions;
using Module.Common.WebUI.Models;
using SmartBreadcrumbs.Nodes;

namespace Module.Common.WebUI.Base {
    public class AppControllerBase : Controller {
        public string ControllerName => HttpContext.GetRouteData().GetController();
        public string ActionName => HttpContext.GetRouteData().GetAction();

        #region authorization

        private UserSession _userSession;
        protected UserSession UserSession => _userSession ??= HttpContext.GetUserSession();

        #endregion

        #region notifications

        protected virtual void SetNotification(OperationResult result) {
            _setNotification(result);
        }

        protected virtual void SetSuccessNotification() {
            _setNotification(OperationResult.Success());
        }

        protected virtual void SetFailureNotification() {
            _setNotification(OperationResult.Failure());
        }

        private void _setNotification(OperationResult operationResult) {
            TempData[GlobalConstant.NotificationKey] = JsonSerializer.Serialize(operationResult,
                new JsonSerializerOptions {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
        }

        #endregion

        protected virtual void SetMetaInfo(MetaViewModel meta) {
            ViewData[GlobalConstant.MetaKey] = meta;
        }

        #region return results

        public RedirectResult RedirectToActionWithQueryString(string action, string queryString) {
            var area = ControllerContext.RouteData.Values.ContainsKey("area")
                ? $"/{ControllerContext.RouteData.Values["area"]}"
                : string.Empty;
            return new RedirectResult(
                $"{area}/{ControllerContext.RouteData.Values["controller"]}/{action}?{queryString}");
        }

        public RedirectResult RedirectToRefererOrAction(string action) {
            if (Request.GetTypedHeaders().Referer != null) {
                return new RedirectResult(Request.GetTypedHeaders().Referer.ToString());
            }

            var area = ControllerContext.RouteData.Values.ContainsKey("area")
                ? $"/{ControllerContext.RouteData.Values["area"]}"
                : string.Empty;
            return new RedirectResult($"{area}/{ControllerContext.RouteData.Values["controller"]}/{action}");
        }

        #endregion

        #region breadcrumbs

        protected void SetBreadcrumbs(BreadcrumbNode breadcrumb) {
            ViewData[GlobalConstant.BreadcrumbKey] = breadcrumb;
        }

        protected void SetBreadcrumbs(string controller, string action, string title, RouteValueDictionary routeValues = null,
            BreadcrumbNode child = null) {
            SetBreadcrumbs(CreateBreadcrumb(controller, action, title, routeValues, child));
        }   

        protected BreadcrumbNode CreateBreadcrumb(string controller, string action, string title,
            RouteValueDictionary routeValues = null, BreadcrumbNode child = null) {
            routeValues ??= new RouteValueDictionary();
            routeValues.Add("area", "");
            var node = new MvcBreadcrumbNode(action, controller, title, areaName: "") {RouteValues = routeValues};
            if (child != null) {
                child.Parent = node;
                return child;
            }

            return node;
        }

        #endregion

        protected virtual void SetLayout(string name) {
            ViewData["_Layout"] = name;
        }
        
        public async Task<bool> HasAccess(string controller, string action, string area = "Admin") {
            if (UserSession.IsRoot) {
                return true;
            }

            if (!UserSession.IsManager && HttpContext.GetRouteData().GetArea().Equals("Admin")) {
                throw new Exception("Access forbidden");
            }

            return await HttpContext.RequestServices.GetService<IPermissionManager>()!
                .HasAccessAsync(HttpContext, UserSession.ManagerDetails.RoleId, new RouteValue(area, controller, action));
        }
    }
}
