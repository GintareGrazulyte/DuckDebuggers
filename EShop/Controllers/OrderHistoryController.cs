using DAL_API;
using DOL.Accounts;
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

            Customer customer = _customerDAO.FindByEmail(currentCustomer.Email); 
            
            return View(customer.Orders);
        }
        [HttpPost]
        public ActionResult IndexPartial(FormCollection fc)
        {
            var orderID = fc["OrderId"];

            return View();
        }
    }
}