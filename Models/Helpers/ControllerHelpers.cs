using System;
using System.Web;

namespace Helpers
{
    public class ControllerHelpers
    {
        // private RouteData routeData;

        public ControllerHelpers()
        {

        }
        public static bool IsControllerName(params string[] ControllerName)
        {
            var httpContext = new HttpContextWrapper(HttpContext.Current);
            var routeData = System.Web.Routing.RouteTable.Routes.GetRouteData(httpContext);
            foreach (var i in ControllerName)
            {
                if (routeData != null && String.Equals(routeData.Values["Controller"].ToString(), i, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;

                }

            }
            return false;
        }
        public static bool IsActionName(params string[] ActionName)
        {
            var httpContext = new HttpContextWrapper(HttpContext.Current);
            var routeData = HttpContext.Current.Request.RequestContext.RouteData.Values["action"]?.ToString().ToLower();
            foreach (var i in ActionName)
            {
                if (string.Equals(routeData, i.ToLower(), StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }

            }
            return false;
        }
        public static bool IsMenuBarActive(string ActionName, string ControllerName)
        {
            var httpContext = new HttpContextWrapper(HttpContext.Current);
            var actionName = HttpContext.Current.Request.RequestContext.RouteData.Values["action"]?.ToString().ToLower();
            var controllerName = System.Web.Routing.RouteTable.Routes.GetRouteData(httpContext)?.Values["Controller"].ToString().ToLower();
            if (string.Equals(actionName, ActionName.ToLower(), StringComparison.CurrentCultureIgnoreCase) &&
                string.Equals(controllerName, ControllerName.ToLower(), StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }

            return false;
        }
    }
}
