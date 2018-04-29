using DAL;
using DAL_API;
using DOL.Accounts;
using DOL.Carts;
using DOL.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EShop.Controllers
{
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
            return View();
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

        public PartialViewResult Customers()
        {
            return PartialView(_customerDAO.GetAll());
        }

        public PartialViewResult Admins()
        {
            return PartialView(_adminDAO);
        }
    }
}