using System;
using System.Net;
using System.Web.Mvc;
using BOL;
using EShop.Attributes;
using BLL_API;
using EShop.Models;
using System.Collections.Generic;
using System.Linq;

namespace EShop.Controllers
{
    [CustomAuthorization(LoginPage = "~/Admin/Login", Roles = "Admin")]
    public class CategoriesController : Controller      //TODO: exception handling -> error messages
    {
        private ICategoryService _categoryService;
        private IPropertyService _propertyService;

        public CategoriesController(ICategoryService categoryService, IPropertyService propertyService)
        {
            _categoryService = categoryService;
            _propertyService = propertyService;
        }

        // GET: Categories
        public ActionResult Index()
        {
            return View(_categoryService.GetAllCategories());
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            try
            {
                var category = _categoryService.GetCategory(id.Value);
                return View(category);
            }
            catch (ArgumentException)
            {
                return HttpNotFound();
            }
        }


        [HttpGet]
        public ActionResult Create()
        {
            AddCategoryViewModel model = new AddCategoryViewModel();
            var properties = _propertyService.GetAllProperties();
            var checkBoxListItems = new List<CheckBoxListItem>();
            properties.ForEach(x => checkBoxListItems.Add(new CheckBoxListItem()
            {
                ID = x.Id,
                Display = x.Name,
                IsChecked = false
            }));

            model.Properties = checkBoxListItems;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AddCategoryViewModel model)
        {
            var selectedProperties = model.Properties.Where(x => x.IsChecked).Select(x => x.ID).ToList();
            if (ModelState.IsValid)
            {
                try
                {
                    _categoryService.CreateCategory(model.Name, selectedProperties);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(model);
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
            if (ModelState.IsValid)
            {
                try
                {
                    _categoryService.UpdateCategory(category);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
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
            try
            {
                _categoryService.DeleteCategory(id);
                return RedirectToAction("Index");
            }
            catch (ArgumentException)
            {
                return HttpNotFound();
            }
        }
    }
}
