using BLL_API;
using BOL.Accounts;
using BOL.Orders;
using EShop.Attributes;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace EShop.Controllers
{
    [CustomAuthorization(LoginPage = "~/Admin/Login", Roles = "Admin")]
    public class UsersListController : Controller
    {
        private IAdminService _adminService;

        public UsersListController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public ViewResult Index(string sortOrder, string searchWord)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            var word = Request["search"];
            var order = Request["order"] == "Date" ? "date_desc" : "Date";
            var customers = _adminService.GetCustomers();

            if (!String.IsNullOrEmpty(word))
            {
                Regex good = new Regex(@"" + word + "", RegexOptions.IgnoreCase);
                customers = customers.Where((x => good.IsMatch(x.Name) || good.IsMatch(x.Surname)
                                                || good.IsMatch(x.Email))).Distinct().ToList();
            }

             var view = new UserListViewModel()
            {
                Admins = _adminService.GetAdmins(),
                Customers = customers
            };

            return View(view);
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

        [HttpPost]
        public ActionResult OrderTable(FormCollection fc)
        {
            int orderID = 0;
            try
            {
                orderID = Convert.ToInt32(fc["OrderId"]);
            }
            catch (FormatException)
            {
                return Content("<html></html>");
                //return RedirectToAction("Index");
            }

            Customer currentCustomer = (Session["Account"] as Customer);
            Order order = currentCustomer.Orders.FirstOrDefault(o => o.Id == orderID);
            return View(order);
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
                catch (Exception ex)    //TODO: create separate exception to handle "Email already exists"
                {
                    Debug.WriteLine(ex.Message);
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(admin);
        }
    }
}