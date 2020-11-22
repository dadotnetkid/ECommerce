using DevExpress.Web.Mvc;
using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models.Repository;

namespace Web.Controllers
{
    public class ReportController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult OrderReport()
        {
            return PartialView();
        }

        public ActionResult OrderReportPartial([ModelBinder(typeof(DevExpressEditorsBinder))] OrderReportVM item)
        {
            rptOrders rpt = new rptOrders();
            try
            {
                var res = unitOfWork.OrdersRepo.Fetch(x =>
                    (x.OrderedDate >= item.DateFrom && x.OrderedDate <= item.DateTo) &&
                    x.Carts.Products.CreatedBy == item.UserId).ToList();
                rpt = new rptOrders()
                {
                    DataSource = res
                };

            }
            catch (Exception e)
            {

            }
            return PartialView(rpt);
        }
    }
}