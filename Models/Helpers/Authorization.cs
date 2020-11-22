using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using Models.Repository;

namespace Helpers
{
    public class RepatriateAuthorization : AuthorizeAttribute
    {
        public string Url { get; set; }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //filterContext.Result = new HttpUnauthorizedResult(); // Try this but i'm not sure
            filterContext.Result = new RedirectResult(Url);
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (this.AuthorizeCore(filterContext.HttpContext))
            {
                base.OnAuthorization(filterContext);
            }
            else
            {
                this.HandleUnauthorizedRequest(filterContext);
            }
        }

    }
    public class OnUserAuthorizationAttribute : AuthorizeAttribute
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            string actionName = ActionName;//HttpContext.Current.Request.RequestContext.RouteData.Values["Action"].ToString();
            string controllerName = ControllerName;// HttpContext.Current.Request.RequestContext.RouteData.Values["Controller"].ToString();
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var users = unitOfWork.UsersRepo.Fetch(m => m.Id == userId).Any(m => m.UserRoles.Any(x => x.Functions.Any(u => u.Action.Contains(actionName))));
            if (HttpContext.Current.User.IsInRoles(Roles.Split(',')))
            {
                return;
            }

            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                if (!users)
                    filterContext.Result = new RedirectToRouteResult(new
                        RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
            }
            base.OnAuthorization(filterContext);
        }
    }
}
