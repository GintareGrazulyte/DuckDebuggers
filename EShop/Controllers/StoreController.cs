using BLL_API;
using BOL;
using BOL.Objects;
using EShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace EShop.Controllers
{
    public class StoreController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IItemQueryService _itemQueryService;

        public StoreController(ICategoryService categoryService, IItemQueryService itemQueryService)
        {
            _categoryService = categoryService;
            _itemQueryService = itemQueryService;
        }
        // GET: Store
        public ActionResult Index(string Search)
        {
            var categories = _categoryService.GetAllCategories();

            var items = (_itemQueryService.GetAllItems()).Where(i => i.CategoryId == null).ToList();
            IEnumerable<Category> noCategory = new List<Category> { new Category { Id = 0, Items = items, Name = "No category" } };
            categories = categories.Concat(noCategory);
            return View(categories);
        }
        [ChildActionOnly]
        public ActionResult CategoryMenu()
        {
            var categories = _categoryService.GetAllCategories();
            return PartialView(categories);

        }
        public ActionResult Browse(int? categoryId)
        {
            if (categoryId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (categoryId == 0)
            {
                var items = (_itemQueryService.GetAllItems()).Where(i => i.CategoryId == null).ToList();
                var noCategory = new Category { Id = 0, Items = items, Name = "No category" };
                return View(noCategory);
            }
            try
            {
                var category = _categoryService.GetCategory(categoryId.Value);
                return View(category);
            }
            catch (ArgumentException)
            {
                return HttpNotFound();
            }
        }
        public ActionResult Details(int? itemId)
        {
            if (itemId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            try
            {
                var item = _itemQueryService.GetItem(itemId.Value);
                return View(item);
            }
            catch (ArgumentException)
            {
                return HttpNotFound();
            }
        }
    }
}