using System;
using System.Net;
using System.Web.Mvc;
using BOL;
using EShop.Attributes;
using BLL_API;
using log4net;

namespace EShop.Controllers
{
    [CustomAuthorization(LoginPage = "~/Admin/Login", Roles = "Admin")]
    public class CategoriesController : Controller      //TODO: exception handling -> error messages
    {
        private static ILog _logger = LogManager.GetLogger(typeof(CategoriesController));

        private ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: Categories
        public ActionResult Index()
        {
            return View(_categoryService.GetAllCategories());
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            _logger.InfoFormat("View details of category with id [{0}]", id);

            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            try
            {
                var category = _categoryService.GetCategory(id.Value);
                _logger.InfoFormat("Get details of category with id [{0}] was successful", id);

                return View(category);
            }
            catch (ArgumentException)
            {
                _logger.InfoFormat("View details of category with id [{0}]", id);
                return HttpNotFound();
            }
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Category category)
        {
            _logger.InfoFormat("Create category with name [{0}]", category.Name);

            if (ModelState.IsValid)
            {
                try
                {
                    _categoryService.CreateCategory(category);

                    _logger.InfoFormat("Create category with name [{0}] was successful", category.Name);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    _logger.Info(ex.Message);
                    ModelState.AddModelError("", ex.Message);
                }
            }
            _logger.InfoFormat("Create category with name [{0}] failed", category.Name);
            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            try
            {
                Category category = _categoryService.GetCategory(id.Value);
                return View(category);
            }
            catch (ArgumentException)
            {
                return HttpNotFound();
            }
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Category category)
        {
            _logger.InfoFormat("Edit category with id [{0}], set name to [{1}]", category.Id, category.Name);

            if (ModelState.IsValid)
            {
                try
                {
                    _categoryService.UpdateCategory(category);

                    _logger.InfoFormat("Update category with id [{0}], set name to [{1}] was successful", 
                        category.Id, category.Name);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    _logger.Info(ex.Message);

                    ModelState.AddModelError("", ex.Message);
                }
            }

            _logger.InfoFormat("Edit category with id [{0}] failed, set name to [{1}]", category.Id, category.Name);

            return View(category);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            try
            {
                Category category = _categoryService.GetCategory(id.Value);
                return View(category);
            }
            catch (ArgumentException)
            {
                return HttpNotFound();
            }
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _logger.InfoFormat("Delete category with id [{0}]", id);

            try
            {
                _categoryService.DeleteCategory(id);

                _logger.InfoFormat("Delete category with id [{0}] was successful", id);

                return RedirectToAction("Index");
            }
            catch (ArgumentException)
            {
                _logger.InfoFormat("Delete category with id [{0}] failed", id);

                return HttpNotFound();
            }
        }
    }
}
