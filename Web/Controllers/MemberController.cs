using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Models;
using Models.ControllerHelpers;
using Models.Repository;
using Models.ViewModels;

namespace Web.Controllers
{
    [RoutePrefix("member")]
    public class MemberController : IdentityController
    {
        // GET: Member
        private UnitOfWork unitOfWork = new UnitOfWork();

        #region Forgot Password
        [AllowAnonymous, Route("change-password")]
        public ActionResult ChangeForgotPasswordViewPartial(string Email, string Token)
        {
            ViewBag.Email = Email;
            ViewBag.Token = Token;
            return View();
        }
        [HttpPost, AllowAnonymous, Route("change-password")]
        public async Task<ActionResult> ChangeForgotPasswordViewPartial(ForgotPassword forgotPassword)
        {
            var user = await UserManager.FindByEmailAsync(forgotPassword.EmailAddress);
            var res = await UserManager.ResetPasswordAsync(user.Id, forgotPassword.Token, forgotPassword.NewPassword);
            return PartialView("ChangeForgotPasswordViewPartial");
        }
        public async Task<PartialViewResult> ForgotPopupControlPartial([ModelBinder(typeof(DevExpressEditorsBinder))] string emailAddress)
        {
            if (emailAddress != null)
            {
                try
                {
                    var user = unitOfWork.UsersRepo.Find(m => m.Email == emailAddress);
                    var token = UserManager.GeneratePasswordResetToken(user?.Id);
                    //var Token = await UserManager.GeneratePasswordResetTokenAsync(user?.Id);
                    var confirmationlink = $"<h3>Forgot Password</h3><br/><a href='{Url.Action("ChangeForgotPasswordViewPartial", "Member", new { Email = user.Email, Token = token }, Request.Url.Scheme)}'>Click here to change your Password</a>";
                    await UserManager.SendEmailAsync(userId: user.Id, subject: "Forgot Password", body: confirmationlink);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            return PartialView();
        }


        #endregion
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View("_login");
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [OutputCache(NoStore = true, Location = System.Web.UI.OutputCacheLocation.None)]
        public async Task<ActionResult> Login([ModelBinder(typeof(DevExpressEditorsBinder))] LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View("_login", model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var user = unitOfWork.UsersRepo.Fetch(x=>x.UserName==model.UserName || x.Email==model.UserName).FirstOrDefault();
           
            if (user == null)
            {
                ModelState.AddModelError("UserName", "Username not exist");
                return View("_login", model);
            }

            var result = await SignInManager.PasswordSignInAsync(user?.UserName, model.Password, model.RememberMe ?? false, shouldLockout: false);
            if (result == SignInStatus.Success)
            {
                if (UserManager.IsInRole(user.Id, "Buyer"))
                {
                    return RedirectToAction("index", "products");
                }
                else
                {
                    return RedirectToAction("products", "admin");
                }
            }

            return View("_login", model);
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register([ModelBinder(typeof(DevExpressEditorsBinder))] RegisterViewModel model)
        {
            var user = new Users()
            {
                Id = Guid.NewGuid().ToString(),
                Email = model.Email,
                UserName = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                AddressLine1 = model.Address,
                PhoneNumber = model.PhoneNumber
            };
            var result = UserManager.Create(user, model.Password);
            
            if (result.Succeeded)
            {
                var type = "Seller";
                if (model.Buyer)
                    type = "Buyer";
                UserManager.AddToRole(user.Id, type);
                return RedirectToAction("login", "member");
            }
           
            return View(model);
        }

        //public ActionResult RegisterPartial(RegisterViewModel model)
        //{

        //}
        //[HttpPost]
        //public async Task<ActionResult> Register(RegisterViewModel model)
        //{
        //    var user = new Users() { Id = Guid.NewGuid().ToString(), UserName = model.Email, Email = model.Email, FirstName = model.FirstName, MiddleName = model.MiddleName, LastName = model.LastName };
        //    var res = await UserManager.CreateAsync(user, model.Password);
        //    return RedirectToAction("Login");
        //}



        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "admin");
        }
        [Route("log-off")]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("index", "products");
        }
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }




        [ValidateInput(false)]
        public ActionResult CaptchaPartial()
        {
            return PartialView("_CaptchaPartial");
        }
    }
}