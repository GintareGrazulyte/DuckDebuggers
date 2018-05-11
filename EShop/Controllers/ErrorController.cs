using System.Web.Mvc;

namespace EShop.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult DefaultError()
        {
            return View();
        }

        public ActionResult NotFoundError()
        {
            return View();
        }

        public ActionResult InternalError()
        {
            return View();
        }
    }
}