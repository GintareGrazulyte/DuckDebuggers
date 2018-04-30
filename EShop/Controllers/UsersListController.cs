using DAL_API;
using DOL.Accounts;
using DOL.Orders;
using EShop.Attributes;
using System;
using System.Linq;
using System.Web.Mvc;

namespace EShop.Controllers
{
    [CustomAuthorization(LoginPage = "~/Admin/Login", Roles = "Admin")]
    public class UsersListController : Controller
    {
        private ICustomerDAO _customerDAO;
        private IAdminDAO _adminDAO;

        public UsersListController(ICustomerDAO customerDAO, IAdminDAO adminDAO)
        {
            _customerDAO = customerDAO;
            _adminDAO = adminDAO;
        }

        // GET: OrderHistory
        public ActionResult Index()
        {
            var view = new UserListViewModel()
            {
                Admins = _adminDAO.GetAll(),
                Customers = _customerDAO.GetAll()
            };
            return View(view);
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