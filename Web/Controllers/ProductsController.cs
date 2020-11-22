using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.DataProcessing;
using DevExpress.Web.Mvc;
using Helpers;
using Microsoft.AspNet.Identity;
using Models;
using Models.Repository;
using PagedList;

namespace Web.Controllers
{
    public class ProductsController : Controller
    {

        UnitOfWork unitOfWork = new UnitOfWork();
        // GET: Products
        public ActionResult Index()
        {

            return View();
        }
        [HttpGet]
        public ActionResult ProductPartials(int page = 1)
        {
            var model = unitOfWork.ProductsRepo.Get();
            ViewBag.page = page + 1;
            return PartialView(model.ToPagedList(page, 12));

        }
        [HttpPost]
        public ActionResult ProductPartials(string search = "", int page = 1)
        {
            var model = unitOfWork.ProductsRepo.Get(x => x.Name.Contains(search) || x.Descriptions.Contains(search));
            ViewBag.page = page + 1;
            ViewBag.search = search;
            return PartialView(model.ToPagedList(page, 12));
        }

        public ActionResult ReviewPartial(int productId)
        {
            ViewBag.productId = productId;

            return PartialView(unitOfWork.ReviewsRepo.Get(x => x.ProductId == productId));
        }
        [ChildActionOnly]
        public ActionResult AddReview([ModelBinder(typeof(DevExpressEditorsBinder))]
            int? productId)
        {
            ViewBag.productId = productId;
            return PartialView();
        }
        [HttpPost]
        public ActionResult WriteReview([ModelBinder(typeof(DevExpressEditorsBinder))]
            Reviews item)
        {
            ViewBag.productId = item.ProductId;
            unitOfWork.ReviewsRepo.Insert(new Reviews()
            {
                ProductId = item.ProductId,
                Rating = item.Rating,
                Review = item.Review,
                UserId = User.Identity.GetUserId(),
                DateCreated = DateTime.Now
            });
            unitOfWork.Save();
            return PartialView("ReviewPartial", unitOfWork.ReviewsRepo.Get(x => x.ProductId == item.ProductId));
        }
        public ActionResult Carts()
        {

            return View();
        }

        public ActionResult CartPartials()
        {
            var userId = User.Identity.GetUserId();
            var model = unitOfWork.CartsRepo.Get(x => x.BuyerId == userId && x.Status != "DONE");
            return PartialView(model);
        }
        [Route("product-details/{productId?}")]
        public ActionResult ProductDetails(int? productId)
        {
            var model = unitOfWork.ProductsRepo.Find(x => x.Id == productId);
            return View(model);
        }
        [Route("add-to-cart/{productId?}/{cartId?}/{qty?}")]
        public ActionResult AddToCart(int? productId, int? cartId, int? qty = 1)
        {
            var userId = User.Identity.GetUserId();
            var cart = unitOfWork.CartsRepo.Find(x => x.Id == cartId || (x.ProductId == productId && x.BuyerId == userId));
            var prod = unitOfWork.ProductsRepo.Find(x => x.Id == productId);

            if (cart == null)
            {
                unitOfWork.CartsRepo.Insert(new Models.Carts()
                {
                    BuyerId = userId,
                    ProductId = productId,
                    QTY = qty,
                    Amount = (prod?.Price ?? 0) * qty,
                    Status = "PENDING"
                });
                unitOfWork.Save();
                return RedirectToAction("carts");
            }
            else
            {
                var oldQty = cart.QTY + qty;
                if (oldQty <= 0)
                {
                    unitOfWork.CartsRepo.Delete(x => x.Id == cart.Id);
                }
                else
                {
                    cart.QTY += qty;
                    cart.Amount = (prod?.Price ?? 0) * cart.QTY;
                }

            }
            unitOfWork.Save();
            var model = unitOfWork.CartsRepo.Get(x => x.BuyerId == userId && x.Status != "DONE");
            return RedirectToAction("CartPartials", model);
        }
        [Route("remove-item-in-cart/{cartid?}")]
        public ActionResult RemoveItemInCart(int? cartId)
        {
            unitOfWork.CartsRepo.Delete(x => x.Id == cartId);
            unitOfWork.Save();
            var userId = User.Identity.GetUserId();
            var model = unitOfWork.CartsRepo.Get(x => x.BuyerId == userId && x.Status != "DONE");
            return PartialView("CartPartials", model);
        }

        [Route("make-purchase")]
        public ActionResult MakePurchase()
        {
            var userId = User.Identity.GetUserId();
            var cart = unitOfWork.CartsRepo.Get(x => x.BuyerId == userId && x.Status != "DONE");
            var orderNumber = new Random().Next(1, 100000000);
            var datePurchase = DateTime.Now;
            foreach (var i in cart)
            {
                i.Status = "DONE";

                unitOfWork.OrdersRepo.Insert(new Models.Orders()
                {
                    OrderedDate = datePurchase,
                    CartId = i.Id,
                    OrderNumber = orderNumber,
                    OrderStatus = new List<OrderStatus>()
                    {
                        new OrderStatus()
                        {
                            Status="Pending to be reviewed"
                        }
                    }
                });
            }

            unitOfWork.Save();

            return RedirectToAction("orders");
        }

        public ActionResult Orders()
        {

            return View();
        }

        public ActionResult OrderPartial()
        {
            var userId = User.Identity.GetUserId();
            var model = unitOfWork.OrdersRepo.Get(x =>
                x.Carts.BuyerId == userId && x.OrderStatus.Any(m => m.Status != "Transaction Complete") && x.isCancel != true);
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult CancelOrder(int? orderId, string reason)
        {
            var order = unitOfWork.OrdersRepo.Find(x => x.Id == orderId);
            order.OrderStatus.Add(new OrderStatus()
            {
                Status = reason
            });
            order.OrderStatus.Add(new OrderStatus()
            {
                Status = "Order is Cancelled"
            });
            order.isCancel = true;
            unitOfWork.Save();
            var userId = User.Identity.GetUserId();
            var model = unitOfWork.OrdersRepo.Get(x =>
                x.Carts.BuyerId == userId && x.OrderStatus.Any(m => m.Status != "Transaction Complete") && x.isCancel != true);
            return PartialView("OrderPartial", model);
        }

        [HttpGet]
        public ActionResult CancelOrder(int? orderId)
        {
            ViewBag.orderId = orderId;
            return PartialView();
        }

        public ActionResult StripePayment()
        {
            var key = "pk_test_3RshjIpKWmwnm0GVvkv23JLH";
            var secret = "sk_test_SAQB5xoKmJry2kMR8m6TOkdX";
            
        }
    }
}