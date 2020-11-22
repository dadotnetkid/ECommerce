using System.Web.Mvc;
using DevExpress.Web.Mvc;

namespace Web.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
      
        [Route("access-denied")]
        public ActionResult AccessDenied()
        {
            return View();
        }
        [Route("page-not-found")]
        public ActionResult PageNotFound()
        {
            return View();
        }
    }
}