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
            Customer customer = _customerDAO.FindByEmail(((Customer)Session["Account"]).Email);
            foreach(var order in customer.Orders)
            {
                //
            }
            return View(customer.Orders);
        }
    }
}