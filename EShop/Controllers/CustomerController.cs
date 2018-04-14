using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOL.Accounts;

namespace EShop.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }

        //GET: Customer/Register
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Customer customer)
        {
            return View(customer);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Customer customer)
        {
            var foundCustomer = new Object();
            if(foundCustomer != null)
            {
                Session["UserId"] = foundCustomer;
                return RedirectToAction("LoggedIn");
            }
            else
            {
                ModelState.AddModelError("", "Wrong email or password");
            }
            return View(customer);
        }

        public ActionResult LoginIn(Object customer)
        {
            if(Session["UserId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
    }
}