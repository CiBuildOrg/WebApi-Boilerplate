using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace App.Infrastructure.Tracing.Dashboard
{
    public static class HtmlHelperExtensions
    {
        public static string MyActionLink(this HtmlHelper helper, string actionName, string controllerName, string linkText)
        {
            var qs = helper.ViewContext.HttpContext.Request.QueryString;
            var dict = new RouteValueDictionary();
            qs.AllKeys.ToList().ForEach(k => dict.Add(k, qs[k]));

            var urlhelper = new UrlHelper(helper.ViewContext.RequestContext);

            var url = urlhelper.Action(actionName, controllerName, dict);
            var tagBuilder = new TagBuilder("a")
            {
                InnerHtml = (!string.IsNullOrEmpty(linkText)) ? HttpUtility.HtmlEncode(linkText) : string.Empty
            };
            tagBuilder.MergeAttribute("href", url);
            return tagBuilder.ToString(TagRenderMode.Normal);

        }
    }
}