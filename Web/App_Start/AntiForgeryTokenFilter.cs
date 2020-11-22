using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.App_Start
{
    public class AntiForgeryTokenFilter : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception is HttpAntiForgeryException)
            {
                var url = HttpContext.Current.Request.Url.AbsoluteUri;
                filterContext.Result = new RedirectResult(url); // whatever the url that you want to redirect to
               

                filterContext.ExceptionHandled = true;
            }
        }
    }
}