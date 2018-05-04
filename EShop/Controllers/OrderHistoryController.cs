using DAL_API;
using BOL.Accounts;
using BOL.Carts;
using BOL.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL_API;

namespace EShop.Controllers
{
    public class OrderHistoryController : Controller
    {
        private ICustomerAccountService _customerAccountService;

        public OrderHistoryController(ICustomerAccountService customerAccountService)
        {
            _customerAccountService = customerAccountService;
        }

        // GET: OrderHistory
        public ActionResult Index()
        {
            int? currentCustomerId = (int)Session["AccountId"];
            if (currentCustomerId == null)
                return RedirectToAction("Index", "Store");

            Customer currentCustomer = _customerAccountService.GetCustomer((int)currentCustomerId);
            if (currentCustomer == null)
                return RedirectToAction("Index", "Store");

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
            }

            int? currentCustomerId = (int)Session["AccountId"];
            if (currentCustomerId == null)
                return Content("<html></html>");

            Customer currentCustomer = _customerAccountService.GetCustomer((int)currentCustomerId);
            Order order = currentCustomer.Orders.FirstOrDefault(o => o.Id == orderID);
            return View(order);
        }
    }
}