using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.RegularExpressions;
using System.Web;

namespace SASTTest.Views;

public static class HtmlHelperExtensions
{
    public static IHtmlContent Bold(this IHtmlHelper htmlHelper, string content)
    {
        return new HtmlString($"<b>{content}</b>");
    }

    public static IHtmlContent Bold_Safe(this IHtmlHelper htmlHelper, string content)
    {
        var encoded = System.Net.WebUtility.HtmlEncode(content);
        return new HtmlString($"<b>{encoded}</b>");
    }

    public static IHtmlContent Italic(this IHtmlHelper htmlHelper, string content)
    {
        return new HtmlString($"<i>{content}</i>");
    }

    public static IHtmlContent Italic_Safe(this IHtmlHelper htmlHelper, string content)
    {
        //System.Net.WebUtility.HtmlEncode();
        var encoded = HttpUtility.HtmlEncode(content);
        return new HtmlString($"<i>{encoded}</i>");
    }
}
