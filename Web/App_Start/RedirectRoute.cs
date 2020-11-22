using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Web
{
    public class RedirectRoute : RouteBase
    {

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var url = httpContext.Request.Headers["HOST"];
            var index = url.IndexOf(".");

            if (index < 0)
            {
                return null;
                //return routeData;
            }

            var subDomain = url.Substring(0, index);

            if (subDomain == "balik-probinsya")
            {
                var routeData = new RouteData(this, new MvcRouteHandler());
                routeData.Values.Add("controller", "Repatriates"); //Goes to the User1Controller class
                routeData.Values.Add("action", "index"); //Goes to the Index action on the User1Controller

                return routeData;
            }

            else
            {
                var routeData = new RouteData(this, new MvcRouteHandler());
                routeData.Values.Add("controller", "admin"); //Goes to the User1Controller class
                routeData.Values.Add("action", "index"); //Goes to the Index action on the User1Controller

                return routeData;
            }

            return null;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            //Implement your formating Url formating here
            return null;
        }
    }
}