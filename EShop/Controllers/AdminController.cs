using System;
using System.Collections.Generic;
using BOL.Accounts;
using DAL_API;
using System.Web.Mvc;
using System.Web.Security;
using EShop.Attributes;
using BLL_API;

namespace EShop.Controllers
{
    public class AdminController : Controller
    {
        private IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [CustomAuthorization(LoginPage = "~/Admin/Login", Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Admin admin, string returnUrl)
        {
            var foundAdmin = _adminService.LoginAdmin(admin);
            if (foundAdmin != null)
            {
                FormsAuthentication.SetAuthCookie(foundAdmin.Email, false);
                Session["AccountId"] = foundAdmin.Id;
                Session["AccountEmail"] = foundAdmin.Email;
                Session["IsAdminAccount"] = true;
                if(returnUrl == null || returnUrl == string.Empty)
                {
                    returnUrl = "Index";
                }
                return Redirect(returnUrl);
            }
            else
            {
                ModelState.AddModelError("", "Wrong email or password");
            }
            return View(admin);
        }

        public ActionResult Logout()
        {
            Session["AccountId"] = null;
            Session["AccountEmail"] = null;
            Session["IsAdminAccount"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //_adminDAO.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}