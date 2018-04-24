using System;
using System.Collections.Generic;
using DOL.Accounts;
using DAL_API;
using System.Web.Mvc;

namespace EShop.Controllers
{
    public class AdminController : Controller
    {
        private IAdminDAO _adminDAO;

        public AdminController(IAdminDAO adminDAO)
        {
            _adminDAO = adminDAO;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Admin customer)
        {
            var foundAdmin = _adminDAO.FindByEmail(customer.Email);
            //TODO needs to hashed, but how do you create an admin then?
            if (foundAdmin != null && foundAdmin.Password == customer.Password)
            {
                Session["Account"] = foundAdmin;
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Wrong email or password");
            }
            return View(customer);
        }
    }
}