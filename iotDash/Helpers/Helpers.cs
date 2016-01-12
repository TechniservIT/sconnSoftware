using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.WebPages;

namespace iotDash.Helpers
{
    public static class Helpers
    {
        public static HelperResult RenderSection(this WebPageBase webPage,
            string name, Func<dynamic, HelperResult> defaultContents)
        {
            if (webPage.IsSectionDefined(name))
            {
                return webPage.RenderSection(name);
            }
            return defaultContents(null);
        }
        
    }

    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString ToggleSwitchFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, bool>> expression)
        {
            return htmlHelper.CheckBoxFor(expression,new { @class="iotoggler" @type ="checkbox", @data_toggle = "toggle", @data_onstyle="success" });
       }
    }


}