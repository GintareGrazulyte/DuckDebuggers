using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOL.Accounts;
using DAL_API;
using EShop.Utils;

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
            if (Session["Customer"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
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
                var foundCustomer = _customerDAO.FindByEmail(customer.Email);
                if(foundCustomer == null)
                {
                    customer.Password = Encryption.SHA256(customer.Password);
                    customer.ConfirmPassword = Encryption.SHA256(customer.ConfirmPassword);
                    _customerDAO.Add(customer);
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError("", "Email already exists!");
                }
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
            if(foundCustomer != null && foundCustomer.Password == Encryption.SHA256(customer.Password))
            {
                Session["Customer"] = foundCustomer;
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Wrong email or password");
            }
            return View(customer);
        }

        public ActionResult Logout()
        {
            Session["Customer"] = null;
            return RedirectToAction("Login");
        }
    }
}