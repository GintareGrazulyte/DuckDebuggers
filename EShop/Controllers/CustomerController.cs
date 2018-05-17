using System.Web.Mvc;
using BOL.Accounts;
using System.Web.Security;
using EShop.Attributes;
using BLL_API;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using EShop.Models;

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
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(customer);
        }

        [CustomAuthorization(LoginPage = "~/Customer/Login", Roles = "Customer")]
        public ActionResult Edit()
        {
            return View(_customerAccountService.GetCustomer((int)Session["AccountId"]));
        }

        [CustomAuthorization(LoginPage = "~/Customer/Login", Roles = "Customer")]
        public ActionResult Details()
        {
            return View(_customerAccountService.GetCustomer((int)Session["AccountId"]));
        }

        //TODO add a separate action to change password
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorization(LoginPage = "~/Customer/Login", Roles = "Customer")]
        public ActionResult Edit(Customer customer)
        {
            ModelState.Remove("Password");
            if (ModelState.IsValid)
            {
                try
                {
                    _customerAccountService.Modify(customer);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)    //TODO: create separate exception to handle "Email already exists"
                {
                    Debug.WriteLine(ex.Message);
                    ModelState.AddModelError("", ex.Message);
                }
            }
            //var errors = ModelState.Values.SelectMany(v => v.Errors);
            return View(customer);
        }

        [CustomAuthorization(LoginPage = "~/Customer/Login", Roles = "Customer")]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [CustomAuthorization(LoginPage = "~/Customer/Login", Roles = "Customer")]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                Customer customer = _customerAccountService.GetCustomer((int)Session["AccountId"]);
                if (!customer.IsCorrectPassword(model.OldPassword))
                {
                    ModelState.AddModelError("", "Wrong old password");
                    return View(model);
                }
                if(!model.IsNewPasswordNew())
                {
                    ModelState.AddModelError("", "New password is the same as old one! Pick a new password");
                    return View(model);
                }
                _customerAccountService.UpdatePassword(customer.Id, model.NewPassword);
                return Redirect("Index");
            }
            return View(model);
        }

        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(Customer customerToLogin, string returnUrl)
        {
            //TODO also check if admin email isn't reserved
            var foundCustomer = _customerAccountService.LoginCustomer(customerToLogin);
            if(foundCustomer != null)
            {
                FormsAuthentication.SetAuthCookie(foundCustomer.Email, false);
                Session["AccountId"] = foundCustomer.Id;
                Session["AccountEmail"] = foundCustomer.Email;
                Session["IsAdminAccount"] = false;
                if (returnUrl == null || returnUrl == string.Empty)
                {
                    returnUrl = "Index";
                }
                return Redirect(returnUrl);
            }

            ModelState.AddModelError("", "Wrong email or password");
            return View(foundCustomer);
        }

        [CustomAuthorization(LoginPage = "~/Customer/Login", Roles = "Customer")]
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