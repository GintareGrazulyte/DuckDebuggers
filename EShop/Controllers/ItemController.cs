﻿using System.Net;
using System.Web.Mvc;
using BOL.Objects;
using EShop.Attributes;
using BLL_API;
using BOL.Accounts;
using System.Web;
using System.IO;
using EShop.Models;
using System.Linq;
using System.Collections.Generic;
using BOL.Property;

namespace EShop.Controllers
{
    [CustomAuthorization(LoginPage = "~/Admin/Login", Roles = "Admin")]
    public class ItemController : Controller    //TODO: exception handling
    {
        private IItemQueryService _itemQueryService;
        private ICategoryService _categoryService;
        private IItemManagementService _itemManagementService;
        private IAdminService _adminService;
        private IPropertyService _propertyService;
        
        public ItemController(IItemQueryService itemQueryService, ICategoryService categoryService, 
            IItemManagementService itemManagementService, IAdminService adminService, IPropertyService propertyService)
        {
            _itemQueryService = itemQueryService;
            _categoryService = categoryService;
            _itemManagementService = itemManagementService;
            _adminService = adminService;
            _propertyService = propertyService;
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
            return View(new ExportedItemsViewModel { ExportedItemsFiles = GetExportedFiles() });
        }

        public void DowloadExportedItemsFile(string fileName, string directoryName)
        {
            DownloadFile(fileName, directoryName);
        }

        public ActionResult DownloadImportExample()
        {
            DownloadFile("ImportExample.xlsx", Server.MapPath("~/Content/Downloads"));
            return View("Import");
        }

        private List<FileInfo> GetExportedFiles()
        {
            var directory = new DirectoryInfo(Server.MapPath("~/Content/Downloads/ExportedItems"));
            return  directory.GetFiles("*.xlsx").ToList();
        }

        private void DownloadFile (string fileName, string directoryName)
        {
            HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ClearContent();
            response.Clear();
            response.ContentType = "text/plain";
            response.AddHeader("Content-Disposition",
                               "attachment; filename=" + fileName + ";");
            response.TransmitFile(Path.Combine(directoryName, fileName));
            response.Flush();
            response.End();
        }

        [HttpPost]
        public ActionResult ImportItemsFromFile([Bind(Include = "file")] HttpPostedFileBase file, bool logInfoNeeded)
        {
            int adminId = (int)Session["AccountId"];

            Admin admin = _adminService.GetAdmin(adminId);

            if (file == null)
            {
                ModelState.AddModelError("", "Choose a file to import");
                return View("Import");
            }

            _itemManagementService.ImportItemsFromFile(admin, Server.MapPath("~/Uploads/Items"), file, 
                Server.MapPath("~/Uploads/Images"), logInfoNeeded);

            return View("Index", _itemQueryService.GetAllItems());
        }

        public ActionResult ExportItemsToFile(bool logInfoNeeded)
        {
            int adminId = (int)Session["AccountId"];

            var admin = _adminService.GetAdmin(adminId);

            var allItems = _itemQueryService.GetAllItems();

            _itemManagementService.ExportAllItemsToFile(admin, allItems, logInfoNeeded, Server.MapPath("~/Content/Downloads/ExportedItems"));

            if (logInfoNeeded)
            {
                return View("Index", allItems);
            }
            return View("Export", new ExportedItemsViewModel { ExportedItemsFiles = GetExportedFiles() });
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
            ViewBag.PropertyId = new SelectList(_propertyService.GetAllProperties(), "Id", "Name");
            return View(new Item());
        }

        // POST: Item/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Price,Description,Image,CategoryId,ItemProperties")] Item item)
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
            ViewBag.CategoryId = new SelectList(_categoryService.GetAllCategories(), "Id", "Name", item.CategoryId);
            return View(item);
        }

        // POST: Item/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Price,Description,CategoryId")] Item item)
        {
            ViewBag.CategoryId = new SelectList(_categoryService.GetAllCategories(), "Id", "Name", item.CategoryId);
            //var selectList = ViewBag.CategoryId as SelectList;
            //var selectedItem = selectList.SelectedValue;
            if (ModelState.IsValid)
            {
                _itemManagementService.UpdateItem(item);
                return RedirectToAction("Index");
            }
            return View(item);
        }

        public ActionResult ChangeImage(int? id)
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

        [HttpPost]
        public ActionResult ChangeImage([Bind(Include="Id, Image")] Item model)
        {
            _itemManagementService.UpdateItemImage(model, Server.MapPath("~/Uploads/Images"));
            return RedirectToAction("Index");
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
