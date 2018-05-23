using BLL_API;
using BOL.Accounts;
using EShop.Attributes;
using EShop.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace EShop.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerAccountService _customerAccountService;

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
                catch (Exception ex)    //TODO: create separate exception to handle "Email already exists"
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
                if (!model.IsNewPasswordNew())
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
            if (foundCustomer != null)
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

        [CustomAuthorization(LoginPage = "~/Admin/Login", Roles = "Admin")]
        public ActionResult GetAllCustomers()
        {
            List<Customer> allCustomers = _customerAccountService.GetCustomers()
                .Select(x => new Customer { Id = x.Id, Name = x.Name, Surname = x.Surname, Email = x.Email, IsActive = x.IsActive })
                .Distinct().ToList();
            return PartialView("../Admin/_Search", allCustomers);
        }

        [CustomAuthorization(LoginPage = "~/Admin/Login", Roles = "Admin")]
        public ActionResult ListCustomers(string Search)
        {
            var searchTerm = Search;
            List<Customer> allCustomers = _customerAccountService.GetCustomers()
                .Select(x => new Customer { Id = x.Id, Name = x.Name, Surname = x.Surname, Email = x.Email, IsActive = x.IsActive })
                .Distinct().ToList();

            List<Customer> foundCustomers;
            if (string.IsNullOrEmpty(searchTerm))
            {
                foundCustomers = allCustomers;
            }
            else
            {
                searchTerm = searchTerm.ToUpper();
                foundCustomers = allCustomers.Where(x => x.Name.ToUpper().Contains(searchTerm) || x.Surname.ToUpper().Contains(searchTerm) || x.Email.ToUpper().Contains(searchTerm) || (x.Name.ToUpper() + " " + x.Surname.ToUpper()).Contains(searchTerm))
                    .Select(x => new Customer { Id = x.Id, Name = x.Name, Surname = x.Surname, Email = x.Email, IsActive = x.IsActive })
                    .Distinct().ToList();
            }

            return PartialView("../Admin/_CustomersList", foundCustomers);
        }

        [CustomAuthorization(LoginPage = "~/Admin/Login", Roles = "Admin")]
        public ActionResult ChangeStatus(int id)
        {
            var account = _customerAccountService.GetCustomer(id);
            if (ModelState.IsValid)
            {
                if (account != null)
                {
                    _customerAccountService.ChangeStatus(account);
                }
            }

            return Redirect("Users", "Admin");
        }
    }
}