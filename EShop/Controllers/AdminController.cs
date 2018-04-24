using System;
using System.Collections.Generic;
using DOL.Accounts;
using DAL_API;
using System.Web.Mvc;
using System.Web.Security;

namespace EShop.Controllers
{
    public class AdminController : Controller
    {
        private IAdminDAO _adminDAO;

        public AdminController(IAdminDAO adminDAO)
        {
            _adminDAO = adminDAO;
        }

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Admin customer, string returnUrl)
        {
            var foundAdmin = _adminDAO.FindByEmail(customer.Email);
            //TODO needs to hashed, but how do you create an admin then?
            if (foundAdmin != null && foundAdmin.Password == customer.Password)
            {
                FormsAuthentication.SetAuthCookie(foundAdmin.Email, false);
                Session["Account"] = foundAdmin;
                return Redirect(returnUrl);
            }
            else
            {
                ModelState.AddModelError("", "Wrong email or password");
            }
            return View(customer);
        }

        public ActionResult Logout()
        {
            Session["Account"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}