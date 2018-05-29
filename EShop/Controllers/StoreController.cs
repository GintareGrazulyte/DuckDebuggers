using BLL_API;
using BOL;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace EShop.Controllers
{
    public class StoreController : Controller
    {
        private static ILog _logger = LogManager.GetLogger(typeof(StoreController));

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
            _logger.Info("Get all categories");

            var categories = _categoryService.GetAllCategories();

            var items = (_itemQueryService.GetAllItems()).Where(i => i.CategoryId == null).ToList();
            if (items.Count != 0)
            {
                IEnumerable<Category> noCategory = new List<Category> { new Category { Id = 0, Items = items, Name = "Uncategorized" } };
                categories = categories.Concat(noCategory);
            }

            _logger.InfoFormat("Categories found : [{0}]", categories.ToList().Count);
            return View(categories);
        }

        [ChildActionOnly]
        public ActionResult CategoryMenu()
        {
            var categories = _categoryService.GetAllCategories();
            return PartialView(categories);

        }
      
        public ActionResult Details(int? itemId)
        {
            _logger.InfoFormat("View details of an item with id [{0}]", itemId);

            if (itemId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            try
            {
                var item = _itemQueryService.GetItem(itemId.Value);

                _logger.InfoFormat("Item with id [{0}] was successfully found", itemId);

                return View(item);
            }
            catch (ArgumentException)
            {
                _logger.InfoFormat("Item with id [{0}] details cannot be displayed", itemId);
                return HttpNotFound();
            }
        }

        public ActionResult ListProducts(string search)
        {
            _logger.InfoFormat("Get all items for search criteria : [{0}]", search);

            var searchTerm = search;
            var allItems = _itemQueryService.GetAllItems();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToUpper();
                try
                {
                    var selectedItemsByName = allItems.Where(x => x.Name.ToUpper().Contains(searchTerm)).ToList();
                    var selectedItemsByCategory = allItems.Where(x => x.Category != null && x.Category.Name.ToUpper().Contains(searchTerm)).ToList();
                    var selectedItems = selectedItemsByName.Union(selectedItemsByCategory);

                    _logger.InfoFormat("Search for citeria [{0}] returned [{1}] items", searchTerm, selectedItems.ToList().Count);

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