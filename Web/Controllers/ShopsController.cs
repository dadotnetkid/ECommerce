using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class ShopsController : Controller
    {
        // GET: Shops
        public ActionResult Index()
        {
            return View();
        }
    }
}