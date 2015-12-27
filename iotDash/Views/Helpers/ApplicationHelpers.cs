using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace iotDash.Views.Helpers
{
    public static class StringExtensions
    {
        public static string Titleize(this string text)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text).ToSentenceCase();
        }

        public static string ToSentenceCase(this string str)
        {
            return Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1]));
        }
    }

    public static class ApplicationHelpers
        {
            public static string BuildBreadcrumbNavigation(this HtmlHelper helper)
            {
                // optional condition: I didn't wanted it to show on home and account controller
                if (helper.ViewContext.RouteData.Values["controller"].ToString() == "Home" ||
                    helper.ViewContext.RouteData.Values["controller"].ToString() == "Account")
                {
                    return string.Empty;
                }

                StringBuilder breadcrumb = new StringBuilder("<ol class=\"breadcrumb\"><li>").Append(helper.ActionLink("Home", "Index", "Home").ToHtmlString()).Append("</li>");


                breadcrumb.Append("<li>");
                breadcrumb.Append(helper.ActionLink(helper.ViewContext.RouteData.Values["controller"].ToString().Titleize(),
                                                   "Index",
                                                   helper.ViewContext.RouteData.Values["controller"].ToString()));
                breadcrumb.Append("</li>");

                if (helper.ViewContext.RouteData.Values["action"].ToString() != "Index")
                {
                    breadcrumb.Append("<li>");
                    breadcrumb.Append(helper.ActionLink(helper.ViewContext.RouteData.Values["action"].ToString().Titleize(),
                                                        helper.ViewContext.RouteData.Values["action"].ToString(),
                                                        helper.ViewContext.RouteData.Values["controller"].ToString()));
                    breadcrumb.Append("</li>");
                }

                return breadcrumb.Append("</ol>").ToString();
            }
        }
    
}