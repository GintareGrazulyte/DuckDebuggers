using DAL_API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EShop.Controllers
{
    public class StoreController : Controller
    {
        private ICategoryDAO _categoryDAO;

        public StoreController(ICategoryDAO categoryDAO)
        {
            _categoryDAO = categoryDAO;
        }
        // GET: Store
        public ActionResult Index()
        {
            var categories = _categoryDAO.GetAll();

            return View(categories);
        }
        [ChildActionOnly]
        public ActionResult CategoryMenu()
        {
            var categories = _categoryDAO.GetAll();
            return PartialView(categories);

        }
        public ActionResult Browse(int categoryId)
        {
            var categoryModel = _categoryDAO.FindById(categoryId);
            return View(categoryModel);
        }
        public ActionResult Details(int itemId, int categoryId)
        {
            var item = _categoryDAO.FindById(categoryId).Items.SingleOrDefault(x => x.Id == itemId);
            return View(item);
        }
    }
}