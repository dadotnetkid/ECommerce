using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models.Repository;
using Microsoft.AspNet.Identity;
using Helpers;

namespace Web.Controllers
{
    [OnUserAuthorization(Roles = "Seller,Administrator,Admin")]
    public class AdminController : Controller
    {

        UnitOfWork unitOfWork = new UnitOfWork();


        public string UserId => User.Identity.GetUserId();
        // GET: Admin
        public ActionResult Index()
        {

            return View();
        }

        #region Products
        public ActionResult Products()
        {
            return View();
        }

        public ActionResult Dashboard()
        {

            return View();
        }
        public ActionResult ProductImagePartial()
        {
            return BinaryImageEditExtension.GetCallbackResult();

        }
        public ActionResult AddEditProductPartial([ModelBinder(typeof(DevExpressEditorsBinder))]
            int? productId)
        {
            var model = unitOfWork.ProductsRepo.Find(x => x.Id == productId);
            return PartialView(model);
        }
        [ValidateInput(false)]
        public ActionResult ProductsCardViewPartial()
        {
            var model = unitOfWork.ProductsRepo.Get(x => x.CreatedBy == UserId);
            return PartialView("_ProductsCardViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductsCardViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] Models.Products item)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to insert the new item in your model

                    unitOfWork.ProductsRepo.Insert(item);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.ProductsRepo.Get(x => x.CreatedBy == UserId);
            return PartialView("_ProductsCardViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult ProductsCardViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] Models.Products item)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to update the item in your model
                    unitOfWork.ProductsRepo.Update(item);

                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.ProductsRepo.Get(x => x.CreatedBy == UserId);
            return PartialView("_ProductsCardViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult ProductsCardViewPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] System.Int32 Id)
        {

            if (Id >= 0)
            {
                try
                {
                    // Insert here a code to delete the item from your model
                    unitOfWork.ProductsRepo.Delete(x => x.Id == Id);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            var model = unitOfWork.ProductsRepo.Get(x => x.CreatedBy == UserId);
            return PartialView("_ProductsCardViewPartial", model);
        }
        #endregion

        [ValidateInput(false)]
        public ActionResult ImageGalleryPartial()
        {
            object model = @"~/content/Images/product-images";
            return PartialView("_ImageGalleryPartial", model);
        }

        public ActionResult Orders()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult OrdersGridViewPartial()
        {
            var model = unitOfWork.OrdersRepo.Get(x => x.Carts.Products.CreatedBy == UserId);
            return PartialView("_OrdersGridViewPartial", model);
        }

        public ActionResult OrderStatus()
        {
            return PartialView();
        }

        [ValidateInput(false)]
        public ActionResult OrderStatusGridViewPartial([ModelBinder(typeof(DevExpressEditorsBinder))] int orderId)
        {
            var model = unitOfWork.OrderStatusRepo.Get(x => x.OrderId == orderId);
            ViewBag.orderId = orderId;
            return PartialView("_OrderStatusGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult OrderStatusGridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] Models.OrderStatus item)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to insert the new item in your model
                    var order = unitOfWork.OrdersRepo.Find(x => x.Id == item.OrderId, "OrderStatus");

                    if (item.Orders.isCompleted == true)
                    {
                        order.OrderStatus.Add(new Models.OrderStatus()
                        {
                            OrderId = item.OrderId,
                            Status = "Your order is completed",

                        });
                    }
                    else
                        order.OrderStatus.Add(new Models.OrderStatus()
                        {
                            OrderId = item.OrderId,
                            Status = item.Status,

                        });
                    order.isCompleted = item.Orders.isCompleted;

                    //unitOfWork.OrderStatusRepo.Insert(item);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.OrderStatusRepo.Get(x => x.OrderId == item.OrderId);
            ViewBag.orderId = item.OrderId;
            return PartialView("_OrderStatusGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult OrderStatusGridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] Models.OrderStatus item)
        {

            if (ModelState.IsValid)
            {
                try
                {

                    //  unitOfWork.OrderStatusRepo.Update(item);
                    var res = unitOfWork.OrderStatusRepo.Find(x => x.Id == item.Id, "Orders");
                    res.Status = item.Status;
                    res.Orders.isCompleted = item.Orders.isCompleted;
                    if (item.Orders.isCompleted == true)
                    {
                        unitOfWork.OrderStatusRepo.Insert(new Models.OrderStatus()
                        {
                            OrderId = item.OrderId,
                            Status = "Order is Completed",

                        });
                    }
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var model = unitOfWork.OrderStatusRepo.Get(x => x.OrderId == item.OrderId);
            ViewBag.orderId = item.OrderId;
            return PartialView("_OrderStatusGridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult OrderStatusGridViewPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] System.Int32 Id, [ModelBinder(typeof(DevExpressEditorsBinder))] int orderId)
        {

            if (Id >= 0)
            {
                try
                {
                    // Insert here a code to delete the item from your model
                    unitOfWork.OrderStatusRepo.Delete(x => x.Id == Id);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            var model = unitOfWork.OrderStatusRepo.Get(x => x.OrderId == orderId);
            ViewBag.orderId = orderId;
            return PartialView("_OrderStatusGridViewPartial", model);
        }

        public ActionResult OrderStatusModal([ModelBinder(typeof(DevExpressEditorsBinder))] int orderId)
        {

            ViewBag.orderId = orderId;
            return PartialView();
        }
    }
}