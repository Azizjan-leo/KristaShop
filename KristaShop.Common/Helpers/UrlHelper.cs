using System;
using System.Text.RegularExpressions;

namespace KristaShop.Common.Helpers
{
    public static class UrlHelper
    {
        public static string GetURL(string controller, string action)
        {
            string url = $"/{controller}/{action}";
            return url;
        }

        public static string GetURL(string controller, string action, string parameters)
        {
            string url = $"/{controller}/{action}?{parameters}";
            return url;
        }

        public static string GetURL(string url)
        {
            return url;
        }

        public static bool CompareUrls(string mainUrl, string secondUrl)
        {
            Regex rg = new Regex(mainUrl, RegexOptions.IgnoreCase);
            return rg.IsMatch(secondUrl);
        }


        /// <summary>
        /// uri string format => "/controller/action"
        /// returns => tuple (controller, action)
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>tuple (controller, action)</returns>
        public static (string, string) DeconstructUri(string uri) {
            var result = uri.Split("/", StringSplitOptions.RemoveEmptyEntries);
            if (result.Length >= 2) {
                return (result[0], result[1]);
            }

            return (string.Empty, string.Empty);
        }

        public static string YoutubeUrlToEmbed(string url) {
            if (string.IsNullOrEmpty(url)) return string.Empty;
            return url.Replace("watch?v=", "embed/");
        }
    }
}