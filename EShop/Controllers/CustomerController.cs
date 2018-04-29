using System.Web.Mvc;
using DOL.Accounts;
using DAL_API;
using EShop.Utils;
using System.Web.Security;
using EShop.Attributes;

namespace EShop.Controllers
{
    public class CustomerController : Controller
    {
        private ICustomerDAO _customerDAO;

        public CustomerController(ICustomerDAO customerDAO)
        {
            _customerDAO = customerDAO;
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
            if (ModelState.IsValid)
            {
                var foundCustomer = _customerDAO.FindByEmail(customer.Email);
                if(foundCustomer == null)
                {
                    customer.Password = Encryption.SHA256(customer.Password);
                    customer.ConfirmPassword = Encryption.SHA256(customer.ConfirmPassword);
                    customer.IsActive = true;
                    _customerDAO.Add(customer);
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError("", "Email already exists!");
                }
            }
            return View(customer);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Customer customer)
        {
            //TODO also check if admin email isn't reserved
            var foundCustomer = _customerDAO.FindByEmail(customer.Email);
            if(foundCustomer != null && foundCustomer.Password == Encryption.SHA256(customer.Password) && foundCustomer.IsActive)
            {
                FormsAuthentication.SetAuthCookie(foundCustomer.Email, false);
                Session["Account"] = foundCustomer;
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Wrong email or password");
            }
            return View(customer);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session["Account"] = null;
            return RedirectToAction("Login");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _customerDAO.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}