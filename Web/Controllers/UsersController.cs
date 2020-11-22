using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Web.Mvc;
using System.Web.Mvc;
using Models.ControllerHelpers;
using Models.Repository;
using Microsoft.AspNet.Identity;
using System.IO;

namespace Web.Controllers
{
    public class UsersController : IdentityController
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        public ActionResult Users()
        {
            return View();
        }

        public ActionResult AddEditUserPartial([ModelBinder(typeof(DevExpressEditorsBinder))]
            string userId)
        {
            var model = unitOfWork.UsersRepo.Find(m => m.Id == userId);
            return PartialView(model);
        }

        [ValidateInput(false)]
        public ActionResult UsersGridViewPartial()
        {
            var model = unitOfWork.UsersRepo.Get(m => m.UserRoles.Any(x => x.Name != "repatriates"));
            return PartialView("_UsersGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UsersGridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))]
            Models.Users item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    item.Id = Guid.NewGuid().ToString();
                    item.UserName = item.Email;
                    var res = UserManager.Create(item, item.Password);
                    if (item.UserImage != null)
                    {
                        var img = System.Drawing.Image.FromStream(new MemoryStream(item.UserImage));
                        img.Save(Server.MapPath($"~/content/images/user-images/{User.Identity.GetUserId()}"));
                    }

                    if (res.Succeeded)
                    {
                        if (item.Roles.Any())
                            foreach (var i in item.Roles)
                            {
                                UserManager.AddToRole(item.Id, i);
                            }

                        unitOfWork.Save();
                    }

                    // Insert here a code to insert the new item in your model
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            var model = unitOfWork.UsersRepo.Get();

            return PartialView("_UsersGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public async System.Threading.Tasks.Task<ActionResult> UsersGridViewPartialUpdate(
            [ModelBinder(typeof(DevExpressEditorsBinder))]
            Models.Users item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to update the item in your model
                    item.UserName = item.Email;
                    var user = unitOfWork.UsersRepo.Find(x => x.Id == item.Id);
                    user.UserName = item.Email;
                    user.Email = item.Email;

                    if (!string.IsNullOrEmpty(item.Password))
                    {
                        await UserManager.ChangePasswordAsync(user, item.Password);
                    }


                    if (item.Roles.Any())
                    {
                        user.UserRoles.Clear();
                        foreach (var i in item.Roles)
                        {
                            user.UserRoles.Add(unitOfWork.UserRolesRepo.Find(x => x.Name == i));
                        }
                    }

                    unitOfWork.Save();

                    if (item.UserImage != null)
                    {
                        var img = System.Drawing.Image.FromStream(new MemoryStream(item.UserImage));
                        img.Save(Server.MapPath($"~/content/images/user-images/{User.Identity.GetUserId()}"));
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            var model = unitOfWork.UsersRepo.Get();
            return PartialView("_UsersGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UsersGridViewPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))]
            System.String Id)
        {
            if (Id != null)
            {
                try
                {
                    // Insert here a code to delete the item from your model
                    unitOfWork.UsersRepo.Delete(m => m.Id == Id);
                    unitOfWork.Save();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            var model = unitOfWork.UsersRepo.Get(m => m.UserRoles.Any(x => x.Name != "repatriates"));
            return PartialView("_UsersGridViewPartial", model);
        }
        public ActionResult UserImageUpload()
        {
            return BinaryImageEditExtension.GetCallbackResult();
        }
    }
}