using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOL.Accounts;
using DAL_API;

namespace EShop.Controllers
{
    public class CustomerController : Controller
    {
        private ICustomerDAO _customerDAO;

        public CustomerController(ICustomerDAO customerDAO)
        {
            _customerDAO = customerDAO;
        }
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
        [ValidateAntiForgeryToken]
        public ActionResult Register(Customer customer)
        {
            if (ModelState.IsValid)
            {
                //TODO check if no duplicate email
                //TODO hash password then store
                _customerDAO.Add(customer);
                return RedirectToAction("Login");
            }
            return View(customer);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Customer customer)
        {
            var foundCustomer = _customerDAO.FindByEmail(customer.Email);
            if(foundCustomer != null && foundCustomer.Password == customer.Password)
            {
                Session["Customer"] = foundCustomer;
                return RedirectToAction("LoggedIn");
            }
            else
            {
                ModelState.AddModelError("", "Wrong email or password");
            }
            return View(customer);
        }

        public ActionResult LoggedIn(Customer customer)
        {
            if(Session["Customer"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult Logout()
        {
            Session["Customer"] = null;
            return RedirectToAction("Login");
        }
    }
}