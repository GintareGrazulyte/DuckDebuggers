using System.Collections.Generic;
using System.Linq;
using BOL.Accounts;
using System.Web.Mvc;
using System.Web.Security;
using EShop.Attributes;
using BLL_API;
using log4net;

namespace EShop.Controllers
{
    public class AdminController : Controller
    {
        private static ILog _logger = LogManager.GetLogger(typeof(AdminController));

        private IAdminService _adminService;
        

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [CustomAuthorization(LoginPage = "~/Admin/Login", Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Admin admin, string returnUrl)
        {
            _logger.InfoFormat("Login : email [{0}].", admin.Email);

            var foundAdmin = _adminService.LoginAdmin(admin);
            if (foundAdmin != null)
            {
                log4net.GlobalContext.Properties["user"] = foundAdmin.Email;
                log4net.GlobalContext.Properties["role"] = "Admin";
                _logger.InfoFormat("Login : email [{0}] was successful.", foundAdmin.Email);

                FormsAuthentication.SetAuthCookie("a"+foundAdmin.Email, false);
                Session["AccountId"] = foundAdmin.Id;
                Session["AccountEmail"] = foundAdmin.Email;
                Session["IsAdminAccount"] = true;
                if(returnUrl == null || returnUrl == string.Empty)
                {
                    returnUrl = "Index";
                }
                return Redirect(returnUrl);
            }
            else
            {
                _logger.InfoFormat("Login : email [{0}] was unsuccessful.", admin.Email);

                ModelState.AddModelError("", "Wrong email or password");
            }
            return View(admin);
        }

        public ActionResult Logout()
        {
            string email = Session["AccountEmail"].ToString();
            _logger.InfoFormat("Logout : email [{0}].", email);

            log4net.GlobalContext.Properties["user"] = null;
            log4net.GlobalContext.Properties["role"] = null;
            Session["AccountId"] = null;
            Session["AccountEmail"] = null;
            Session["IsAdminAccount"] = null;
            FormsAuthentication.SignOut();

            _logger.InfoFormat("Logout successful : email [{0}].", email);

            return RedirectToAction("Login");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //_adminDAO.Dispose();
            }
            base.Dispose(disposing);
        }

        [CustomAuthorization(LoginPage = "~/Admin/Login", Roles = "Admin")]
        public ActionResult ListAdmins()
        {
            _logger.Info("Getting admins");

            List<Admin> allAdmins = _adminService.GetAdmins()
                .Select(x => new Admin { Id = x.Id, Name = x.Name, Surname = x.Surname, Email = x.Email, IsActive = x.IsActive })
                .Distinct().ToList();

            _logger.InfoFormat("Admins found: [{0}]", allAdmins.Count);

            return PartialView("_AdminsList", allAdmins);
        }

        

        [CustomAuthorization(LoginPage = "~/Admin/Login", Roles = "Admin")]
        public ActionResult Users() => View();
    }
}