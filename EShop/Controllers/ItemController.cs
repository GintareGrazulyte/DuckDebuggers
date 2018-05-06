﻿using System.Net;
using System.Web.Mvc;
using DAL_API;
using BOL.Objects;
using EShop.Attributes;
using BLL_API;
using System.Collections.Generic;
using System;
using BOL.Accounts;

namespace EShop.Controllers
{
    [CustomAuthorization(LoginPage = "~/Admin/Login", Roles = "Admin")]
    public class ItemController : Controller    //TODO: exception handling
    {
        private IItemQueryService _itemQueryService;
        private ICategoryService _categoryService;
        private IItemManagementService _itemManagementService;
        private IAdminService _adminService;

        public ItemController(IItemQueryService itemQueryService, ICategoryService categoryService, 
            IItemManagementService itemManagementService, IAdminService adminService)
        {
            _itemQueryService = itemQueryService;
            _categoryService = categoryService;
            _itemManagementService = itemManagementService;
            _adminService = adminService;
        }

        // GET: Item
        public ActionResult Index()
        {
            return View(_itemQueryService.GetAllItems());
        }

        public ActionResult Import()
        {
            return View();
        }

        public ActionResult Export()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportItemsFromFile(string path)
        {
            //TODO check, smth is wrong
            int? adminId = (int?)Session["AccountId"];

            if (adminId == null)
            {
                //TODO handle
            }

            try
            {
                _itemManagementService.SetDocument(path);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View("Import");
            }


            Admin admin = _adminService.GetAdmin((int)adminId);

            _itemManagementService.ImportItemsFromFile(admin);

            return View("Index", _itemQueryService.GetAllItems());
        }

        [HttpPost]
        public ActionResult ExportItemsToFile(string path)
        {
            if (System.IO.File.Exists(path))
            {
                ModelState.AddModelError("", "File alredy exists");
                return View("Export");
            }

            _itemManagementService.ExportAllItemsToFile(path);

            //TODO: inform that action completed successfully somehow differently
            ModelState.AddModelError("", "Successfully exported");
            return View("Export");
        }

        // GET: Item/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = _itemQueryService.GetItem(id.Value);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: Item/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_categoryService.GetAllCategories(), "Id", "Name");
            return View();
        }

        // POST: Item/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Price,Description,Image,CategoryId")] Item item)
        {
            ViewBag.CategoryId = new SelectList(_categoryService.GetAllCategories(), "Id", "Name");
            if (ModelState.IsValid)
            {
                _itemManagementService.CreateItemWithImage(item, Server.MapPath("~/Uploads/Images"));
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(_categoryService.GetAllCategories(), "Id", "Name", item.CategoryId);
            return View(item);
        }

        // GET: Item/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = _itemQueryService.GetItem(id.Value);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Item/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Price,Description")] Item item)
        {
            if (ModelState.IsValid)
            {
                _itemManagementService.UpdateItem(item);
                return RedirectToAction("Index");
            }
            return View(item);
        }

        // GET: Item/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = _itemQueryService.GetItem(id.Value);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Item/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _itemManagementService.DeleteItem(id);
            return RedirectToAction("Index");
        }
    }
}
