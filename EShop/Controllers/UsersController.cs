using BLL_API;
using BOL.Accounts;
using EShop.Attributes;
using EShop.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;

namespace EShop.Controllers
{
    [CustomAuthorization(LoginPage = "~/Admin/Login", Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly IAdminService _adminService;

        public UsersController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAllCustomers()
        {
            List<Customer> allCustomers = _adminService.GetCustomers()
                .Select(x => new Customer { Id = x.Id, Name = x.Name, Surname = x.Surname, Email = x.Email, IsActive = x.IsActive })
                .Distinct().ToList();
            return PartialView("_Search", allCustomers);
        }

        public ActionResult ListAdmins()
        {
            List<Admin> allAdmins = _adminService.GetAdmins()
                .Select(x => new Admin { Id = x.Id, Name = x.Name, Surname = x.Surname, Email = x.Email, IsActive = x.IsActive })
                .Distinct().ToList();
            return PartialView("_AdminsList", allAdmins);
        }

        public ActionResult ListCustomers(string Search)
        {
            //var searchTerm = Request["search"];
            var searchTerm = Search;
            List<Customer> allCustomers = _adminService.GetCustomers()
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

            return PartialView("_CustomersList",foundCustomers);
        }

        public ActionResult ChangeStatus(int id)
        {
            var account = _adminService.GetCustomer(id);
            if (ModelState.IsValid)
            {
                if (account != null)
                {
                    _adminService.ChangeStatus(account);
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Admin admin)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _adminService.CreateAdmin(admin);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(admin);
        }
    }
}