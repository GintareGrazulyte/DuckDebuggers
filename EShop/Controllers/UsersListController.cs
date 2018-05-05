using BLL_API;
using BOL.Accounts;
using BOL.Orders;
using EShop.Attributes;
using System;
using System.Linq;
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

        // GET: OrderHistory
        public ActionResult Index()
        {
            var view = new UserListViewModel()
            {
                Admins = _adminService.GetAdmins(),
                Customers = _adminService.GetCustomers()
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
    }
}