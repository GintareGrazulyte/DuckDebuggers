using System.Web.Mvc;
using BOL.Accounts;
using DAL_API;
using System.Web.Security;
using EShop.Attributes;
using BOL.Utils;
using BLL_API;
using System;
using System.Diagnostics;

namespace EShop.Controllers
{
    public class CustomerController : Controller
    {
        private ICustomerAccountService _customerAccountService;

        public CustomerController(ICustomerAccountService customerAccountService)
        {
            _customerAccountService = customerAccountService;
        }
        

        [CustomAuthorization(LoginPage = "~/Customer/Login", Roles = "Customer")]
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
                try
                {
                    _customerAccountService.CreateCustomer(customer);
                    return RedirectToAction("Login");
                }
                catch(Exception ex)    //TODO: create separate exception to handle "Email already exists"
                {
                    Debug.WriteLine(ex.Message);
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
        public ActionResult Login(Customer customerToLogin)
        {
            //TODO also check if admin email isn't reserved
            var foundCustomer = _customerAccountService.LoginCustomer(customerToLogin);
            if(foundCustomer != null)
            {
                FormsAuthentication.SetAuthCookie(foundCustomer.Email, false);
                Session["AccountId"] = foundCustomer.Id;
                Session["AccountEmail"] = foundCustomer.Email;
                Session["IsAdminAccount"] = false;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Wrong email or password");
            return View(foundCustomer);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session["AccountId"] = null;
            Session["AccountEmail"] = null;
            Session["IsAdminAccount"] = null;
            return RedirectToAction("Login");
        }
    }
}