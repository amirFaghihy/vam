using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Aban.Dashboard.Helpers
{
    public static class HMTLHelperExtensions
    {
        public static string IsSelected(
            this IHtmlHelper html,
            string? controller = null,
            string? action = null,
            string? cssClass = null)
        {

            if (String.IsNullOrEmpty(cssClass))
                cssClass = "active";

            string? currentAction = html.ViewContext.RouteData.Values["action"] as string;
            string? currentController = html.ViewContext.RouteData.Values["controller"] as string;

            if (String.IsNullOrEmpty(controller))
                controller = currentController;

            if (String.IsNullOrEmpty(action))
                action = currentAction;

            return controller == currentController && action == currentAction ?
                cssClass : String.Empty;
        }

        public static string PageClass(this IHtmlHelper html)
        {
            string? currentAction = html.ViewContext.RouteData.Values["action"] as string;
            return string.IsNullOrEmpty(currentAction) ? "" : currentAction;
        }

    }
}
