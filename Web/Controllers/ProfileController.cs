using DevExpress.Web.Mvc;
using Microsoft.AspNet.Identity;
using Models;
using Models.ControllerHelpers;
using Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class ProfileController : IdentityController
    {
        string UserId => User.Identity.GetUserId();
        UnitOfWork unitOfWork = new UnitOfWork();
        // GET: Profile
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AccountOverview()
        {
            var user = unitOfWork.UsersRepo.Find(x => x.Id == UserId);
            return PartialView(user);
        }
        [HttpGet]
        public ActionResult EditProfile()
        {

            return PartialView(unitOfWork.UsersRepo.Find(x => x.Id == UserId));
        }
        [HttpPost]
        public ActionResult EditProfile([ModelBinder(typeof(DevExpressEditorsBinder))] Users model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(model);
            }



            return PartialView(model);
        }
        [HttpGet]
        public ActionResult ResetPassword()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult ResetPassword(string newPassword)
        {
            var token = UserManager.GeneratePasswordResetToken(UserId);
            var res = UserManager.ResetPassword(UserId, token, newPassword);
            var resultStatus = "Password Successfully Changed";
            var resultClass = "alert-success";
            if (!res.Succeeded)
            {
                resultStatus = "Error in changing password! Try again";
                resultStatus = "alert-danger";
            }

            ViewBag.resultStatus = resultStatus;
            ViewBag.resultClass = resultClass;
            return PartialView();
        }
    }
}