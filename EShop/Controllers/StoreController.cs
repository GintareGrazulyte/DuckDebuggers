using BLL_API;
using BOL;
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
        public ActionResult Index()
        {
            var categories = _categoryService.GetAllCategories();

            var items = (_itemQueryService.GetAllItems()).Where(i => i.CategoryId == null).ToList();
            if (items.Count != 0)
            {
                IEnumerable<Category> noCategory = new List<Category> { new Category { Id = 0, Items = items, Name = "Uncategorized" } };
                categories = categories.Concat(noCategory);
            }
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
                var noCategory = new Category { Id = 0, Items = items, Name = "Uncategorized" };
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

        public ActionResult ListProducts(string Search)
        {
            var searchTerm = Search;
            var allItems = _itemQueryService.GetAllItems();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToUpper();
                try
                {
                    var selectedItemsByName = allItems.Where(x => x.Name.ToUpper().Contains(searchTerm)).ToList();
                    var selectedItemsByCategory = allItems.Where(x => x.Category != null && x.Category.Name.ToUpper().Contains(searchTerm)).ToList();
                    var selectedItems = selectedItemsByName.Union(selectedItemsByCategory);
                    return PartialView("_Products", selectedItems);
                }
                catch (NullReferenceException)
                {
                    //do nth
                }
                
            }
            return PartialView("_Products", allItems);
        }
    }
}