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
    public class OrderHistoryController : Controller
    {
        private ICustomerDAO _customerDAO;

        public OrderHistoryController(ICustomerDAO customerDAO)
        {
            _customerDAO = customerDAO;
        }

        // GET: OrderHistory
        public ActionResult Index()
        {
            Customer currentCustomer = (Session["Account"] as Customer);
            if (currentCustomer == null)
                return RedirectToAction("Index", "Store");

            //Customer customer = _customerDAO.FindByEmail(currentCustomer.Email); 
            if (currentCustomer.Orders.Count == 0)
                return RedirectToAction("NoOrders");
            return View(currentCustomer.Orders);
        }
        public ActionResult NoOrders()
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
    }
}