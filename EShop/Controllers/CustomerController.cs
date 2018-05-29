using BLL_API;
using BOL.Accounts;
using EShop.Attributes;
using EShop.Models;
using log4net;
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
        private static ILog _logger = LogManager.GetLogger(typeof(CustomerController));

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
            _logger.InfoFormat("Registration of user. Name : [{0}], surname : [{1}], email : [{2}].", 
                customer.Name, customer.Surname, customer.Email);
            if (ModelState.IsValid)
            {
                try
                {
                    _customerAccountService.CreateCustomer(customer);

                    _logger.InfoFormat("Registration of user was successful. Name : [{0}], surname : [{1}], email : [{2}].",
                        customer.Name, customer.Surname, customer.Email);

                    return RedirectToAction("Login");
                }
                catch (Exception ex)    //TODO: create separate exception to handle "Email already exists"
                {
                    string exMessage = ex.Message;
                    _logger.Info(exMessage);

                    Debug.WriteLine(exMessage);
                    ModelState.AddModelError("", exMessage);
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
            _logger.Info("Update customer information");

            ModelState.Remove("Password");
            if (ModelState.IsValid)
            {
                try
                {
                    _customerAccountService.Modify(customer);

                    _logger.Info("Update user information was successful");

                    return RedirectToAction("Index");
                }
                catch (Exception ex)    //TODO: create separate exception to handle "Email already exists"
                {
                    _logger.Info(ex);
                    Debug.WriteLine(ex.Message);
                    ModelState.AddModelError("", ex.Message);
                }
            }
            //var errors = ModelState.Values.SelectMany(v => v.Errors);
            _logger.Info("Update user information change failed");
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
            _logger.Info("Change password");

            if (ModelState.IsValid)
            {
                Customer customer = _customerAccountService.GetCustomer((int)Session["AccountId"]);
                if (!customer.IsCorrectPassword(model.OldPassword))
                {
                    var errMessage = "wrong old password";
                    _logger.Info(errMessage);

                    ModelState.AddModelError("", errMessage);
                    return View(model);
                }
                if (!model.IsNewPasswordNew())
                {
                    var errMessage = "New password is the same as old one";
                    _logger.Info(errMessage);

                    ModelState.AddModelError("", errMessage + "! Pick a new password");
                    return View(model);
                }
                _customerAccountService.UpdatePassword(customer.Id, model.NewPassword);

                _logger.Info("Password change was successful");

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
            _logger.InfoFormat("Login : email [{0}].", customerToLogin.Email);

            //TODO also check if admin email isn't reserved
            var foundCustomer = _customerAccountService.LoginCustomer(customerToLogin);
            if (foundCustomer != null)
            {
                log4net.GlobalContext.Properties["user"] = foundCustomer.Email;
                log4net.GlobalContext.Properties["role"] = "User";
                _logger.InfoFormat("Login : email [{0}] was successful.", foundCustomer.Email);

                FormsAuthentication.SetAuthCookie("c"+foundCustomer.Email, false);
                Session["AccountId"] = foundCustomer.Id;
                Session["AccountEmail"] = foundCustomer.Email;
                Session["IsAdminAccount"] = false;
                if (returnUrl == null || returnUrl == string.Empty)
                {
                    return RedirectToAction("Index", "Store");
                }
                return Redirect(returnUrl);
            }

            _logger.InfoFormat("Login : email [{0}] was unsuccessful.", customerToLogin.Email);

            ModelState.AddModelError("", "Wrong email or password");
            return View(foundCustomer);
        }

        [CustomAuthorization(LoginPage = "~/Customer/Login", Roles = "Customer")]
        public ActionResult Logout()
        {
            string email = Session["AccountEmail"].ToString();
            _logger.InfoFormat("Logout : email [{0}].", email);

            FormsAuthentication.SignOut();
            Session["AccountId"] = null;
            Session["AccountEmail"] = null;
            Session["IsAdminAccount"] = null;

            _logger.InfoFormat("Logout successful : email [{0}].", email);

            return RedirectToAction("Login");
        }

        [CustomAuthorization(LoginPage = "~/Admin/Login", Roles = "Admin")]
        public ActionResult GetAllCustomers()
        {
            _logger.Info("Getting customers");

            List<Customer> allCustomers = _customerAccountService.GetCustomers()
                .Select(x => new Customer { Id = x.Id, Name = x.Name, Surname = x.Surname, Email = x.Email, IsActive = x.IsActive })
                .Distinct().ToList();

            _logger.InfoFormat("Users found: [{0}]", allCustomers.Count);

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

            _logger.InfoFormat("Change status [{0}] of a user with id [{1}]", id, account.IsActive);

            if (ModelState.IsValid)
            {
                if (account != null)
                {
                    _customerAccountService.ChangeStatus(account);

                    _logger.InfoFormat("Changed status of a user with id [{0}] to [{1}] ", id, account.IsActive);
                }
            }

            _logger.InfoFormat("Status change for user with id [{0}] failed.", id);
            return RedirectToAction("Users", "Admin");
        }
    }
}