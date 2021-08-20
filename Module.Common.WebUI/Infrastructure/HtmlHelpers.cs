using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using KristaShop.Common.Helpers;
using KristaShop.Common.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Module.Common.WebUI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Module.Common.WebUI.Infrastructure
{
    public static class HtmlHelpers
    {
        public static string IsActionSelected(this IHtmlHelper html, string controller, string action, string cssClass = null) {
            if (string.IsNullOrEmpty(cssClass))
                cssClass = "active";

            var currentAction = (string) html.ViewContext.RouteData.Values["action"];
            var currentController = (string) html.ViewContext.RouteData.Values["controller"];

            if (string.IsNullOrEmpty(controller))
                controller = currentController;

            if (string.IsNullOrEmpty(action))
                action = currentAction;

            return controller == currentController && action == currentAction ? cssClass : string.Empty;
        }

        public static string IsControllerAndActionSelected(this IHtmlHelper html, string controller, string action, string cssClass = null) {
            if (string.IsNullOrEmpty(cssClass))
                cssClass = "active";

            var currentController = (string)html.ViewContext.RouteData.Values["controller"];
            var currentAction = (string)html.ViewContext.RouteData.Values["action"];

            return controller == currentController && action == currentAction ? cssClass : string.Empty;
        }

        public static string IsControllerSelected(this IHtmlHelper html, string controller, string cssClass = null) {
            if (string.IsNullOrEmpty(cssClass))
                cssClass = "active";

            var currentController = (string) html.ViewContext.RouteData.Values["controller"];

            if (string.IsNullOrEmpty(controller))
                controller = currentController;

            return controller == currentController ? cssClass : string.Empty;
        }

        public static string IsControllerSelected(this IHtmlHelper html, List<string> controllers, string cssClass = null) {
            if (string.IsNullOrEmpty(cssClass))
                cssClass = "active";

            var currentController = (string)html.ViewContext.RouteData.Values["controller"];

            if (controllers == null || !controllers.Any())
                return "";

            return controllers.Contains(currentController) ? cssClass : string.Empty;
        }
        
        public static string IsAreaSelected(this IHtmlHelper html, string area, string cssClass = null) {
            if (string.IsNullOrEmpty(cssClass))
                cssClass = "active";

            var currentArea = (string) html.ViewContext.RouteData.Values["area"];

            if (string.IsNullOrEmpty(area))
                area = currentArea;

            return area == currentArea ? cssClass : string.Empty;
        }
        
        public static string AddValue(this IHtmlHelper html, bool allowAdd, string value) {
            return allowAdd ? value : string.Empty;
        }

        public static string AddValue(this IHtmlHelper html, bool allowAdd, string value, string value2) {
            return allowAdd ? value : value2;
        }

        public static string RemoveTags(this IHtmlHelper htmlHelper, string html) {
            return string.IsNullOrEmpty(html) ? string.Empty : Regex.Replace(html, "<.*?>", String.Empty);
        }

        private static string _version = string.Empty;
        public static string GetFileVersion(this IHtmlHelper htmlHelper, int length = 50) {
            if (string.IsNullOrEmpty(_version)) {
                _version = Generator.NewString(length);
            }

            return _version;
        }

        public static IHtmlContent DecodeJsString(this IHtmlHelper htmlHelper, string encodedString) {
            return htmlHelper.Raw(HttpUtility.JavaScriptStringEncode(encodedString));
        }

        public static IHtmlContent AsJsObject(this IHtmlHelper htmlHelper, object value) {
            return htmlHelper.Raw(JsonConvert.SerializeObject(value, new JsonSerializerSettings{ ContractResolver = new CamelCasePropertyNamesContractResolver()}));
        }

        public static IHtmlContent GetNotification(this IHtmlHelper htmlHelper) {
            if (htmlHelper.TempData[GlobalConstant.NotificationKey] != null) {
                return htmlHelper.DecodeJsString(htmlHelper.TempData[GlobalConstant.NotificationKey].ToString());
            }
            return new StringHtmlContent(string.Empty);
        }

        public static void IgnoreNavbarShift(this IHtmlHelper htmlHelper, bool canIgnore = true) {
            htmlHelper.TempData["IgnoreNavbarShift"] = canIgnore;
        }

        public static bool CanIgnoreNavbarShift(this IHtmlHelper htmlHelper) {
            var value = htmlHelper.TempData["IgnoreNavbarShift"];
            return Convert.ToBoolean(value);
        }

        public static IHtmlContent SetMetaInfo(this IHtmlHelper htmlHelper) {
            if (!(htmlHelper.ViewData[GlobalConstant.MetaKey] is MetaViewModel meta)) {
                return htmlHelper.Raw(string.Empty);
            }

            var stringBuilder = new StringBuilder();

            if (!string.IsNullOrEmpty(meta.Description)) {
                stringBuilder.Append("<meta name='description' content='").Append(meta.Description).Append("'>");
                stringBuilder.Append(Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(meta.Keywords)) {
                stringBuilder.Append("<meta name='keywords' content ='").Append(meta.Keywords).Append("'>");
            }

            return htmlHelper.Raw(stringBuilder.ToString());
        }
    }
}