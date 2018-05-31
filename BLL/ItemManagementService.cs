using BLL_API;
using BOL;
using BOL.Accounts;
using BOL.Objects;
using DAL_API;
using Mehdime.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Linq;
using BOL.Property;
using System.Text.RegularExpressions;

namespace BLL
{
    public class ItemManagementService : IItemManagementService
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly IItemRepository _itemRepository;
        private readonly IPropertyService _propertyService;
        private readonly IFileLoader _fileLoader;
        private readonly IImportService _importService;
        private readonly IEmailService _emailService;
        private readonly ICategoryService _categoryService;

        public ItemManagementService(IDbContextScopeFactory dbContextScopeFactory, IItemRepository itemRepository,
                                        IImportService importService, IFileLoader fileLoader, IEmailService emailService,
                                        ICategoryService categoryService, IPropertyService propertyService)
        {
            _dbContextScopeFactory = dbContextScopeFactory ?? throw new ArgumentNullException("dbContextScopeFactory");
            _itemRepository = itemRepository ?? throw new ArgumentNullException("itemRepository");
            _fileLoader = fileLoader ?? throw new ArgumentNullException("fileLoader");
            _importService = importService ?? throw new ArgumentNullException("importService");
            _emailService = emailService ?? throw new ArgumentNullException("emailService");
            _categoryService = categoryService ?? throw new ArgumentNullException("categoryService");
            _propertyService = propertyService ?? throw new ArgumentNullException("propertyService");
        }

        private string CreateItemUrl(Item itemToCreate, string folderToImage)
        {
            if (String.IsNullOrEmpty(itemToCreate.ImageUrl) && itemToCreate.Image != null)
            {
                //TODO would be nice to have a not hardcoded int function
                string[] directories = folderToImage.Split(Path.DirectorySeparatorChar);
                //TODO check image format before loading to server
                string result = Path.DirectorySeparatorChar + Path.Combine(directories[directories.Length - 2], Path.Combine(directories[directories.Length-1], _fileLoader.Load(folderToImage, itemToCreate.Image)));
                if (!IsImageUrlCorrect(result))
                {
                    throw new Exception("Incorrect image format");
                }
                return result;
            }
            else if((!String.IsNullOrEmpty(itemToCreate.ImageUrl) && itemToCreate.Image == null) ||
                (!String.IsNullOrEmpty(itemToCreate.ImageUrl) && itemToCreate.Image != null))
            {
                if (!IsImageUrlCorrect(itemToCreate.ImageUrl))
                {
                    throw new Exception("Incorrect image format");
                }
                return itemToCreate.ImageUrl;
            }
            else
            {
                return null;
            }
        }

        private bool IsImageUrlCorrect(string url)
        {
            return Regex.IsMatch(url, @"(\.png|\.jpg|\.jpeg|\.gif){1}$");
        }

        public async Task ImportItemsFromFile(Admin admin, string folderToFile, HttpPostedFileBase file,
            string imagesFolder, bool logInfoNeeded)
        {
            await Task.Run(() =>
            {
                var fileName = _fileLoader.Load(folderToFile, file);

                var items = _importService.ImportItemsFromFile(Path.Combine(folderToFile, fileName));


                string logInfo = "";

                var categories = _categoryService.GetAllCategories();

              
                int addedCount = 0;
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].Name == null)
                    {
                        logInfo += "<" + items[i].Name + "> is not added as name cannot be empty" +
                        Environment.NewLine;
                        continue;
                    }
                    if (items[i].Title == null)
                    {
                        logInfo += "<" + items[i].Name + "> is not added as title cannot be empty" +
                        Environment.NewLine;
                        continue;
                    }
                    if (items[i].SKUCode == null)
                    {
                        logInfo += "<" + items[i].Name + "> is not added as SKU code cannot be empty" +
                        Environment.NewLine;
                        continue;
                    }
                    if (items[i].Price <= 0)
                    {
                        logInfo += "<" + items[i].Name + "> is not added as price <" +
                            items[i].Price + "> is not valid" + Environment.NewLine;
                        continue;
                    }
                    var itemPropertiesToAdd = new HashSet<ItemProperty>();
                    //NOTE: items[i].Category should be nulled 
                    Category categoryToAdd = null;
                    if (items[i].Category.Name != null && 
                        (categoryToAdd = categories.FirstOrDefault(c => c.Name == items[i].Category.Name)) == null)
                    {
                        logInfo += "<" + items[i].Name + "> category is set to NULL as category <" + 
                            items[i].Category.Name + "> does not exist" + Environment.NewLine;
                        items[i].Category = null;
                    }
                    else
                    {
                        items[i].Category = null;
                        items[i].CategoryId = categoryToAdd.Id;

                        var categoryProperties = categoryToAdd.Properties;
                        var propertiesToAdd = items[i].ItemProperties.ToList();

                        foreach (var categoryproperty in categoryProperties)
                        {
                            var propertyToAdd = propertiesToAdd.FirstOrDefault(x => x.Property.Name == categoryproperty.Name);
                            if (propertyToAdd == null)
                            {
                                propertyToAdd = new ItemProperty();
                            }
                            itemPropertiesToAdd.Add(new ItemProperty() { PropertyId = categoryproperty.Id, Value = propertyToAdd.Value });
                        }
                    }

                    items[i].ItemProperties = itemPropertiesToAdd;

                    try
                    {
                        CreateItemWithImage(items[i], imagesFolder);
                        addedCount++;
                    }
                    catch (Exception e)
                    {
                        logInfo = "Unable to save item <" + items[i].Name + "> to database. Exception message: " + e.Message +
                        Environment.NewLine;
                    }

                }
                if (logInfoNeeded)
                {
                    logInfo += addedCount + "/" + items.Count + " items were successfully added";

                    var email = new Email()
                    {
                        ToName = admin.Name,
                        ToAddress = admin.Email,
                        Subject = "Import",
                        Body = logInfo,
                        AttachmentPath = ""
                    };
                    _emailService.SendEmail(email);
                }
            });
        }

        public async Task ExportAllItemsToFile(Admin admin, IEnumerable<Item> items, bool logInfoNeeded, string folderToSave)
        {
            await Task.Run(() =>
            {
                var attachmentPath = _importService.ExportItemsToFile(items, folderToSave);

                if (logInfoNeeded)
                {
                    var email = new Email()
                    {
                        ToName = admin.Name,
                        ToAddress = admin.Email,
                        Subject = "Export",
                        Body = "",
                        AttachmentPath = attachmentPath
                    };

                    email.Body = "All of the items were successfully exported";

                    _emailService.SendEmail(email);
                }

            });
        }

        public void CreateItemWithImage(Item itemToCreate, string folderToImage)
        {
            using (var dbContextScope = _dbContextScopeFactory.Create())
            {
                Item foundItem = _itemRepository.FindById(itemToCreate.Id);
                if (foundItem != null)
                    throw new Exception(); //TODO: item already exists
                try
                {
                    itemToCreate.ImageUrl = CreateItemUrl(itemToCreate, folderToImage);
                }
                catch
                {
                    throw;
                }
                _itemRepository.Add(itemToCreate);
                dbContextScope.SaveChanges();
            }
            
        }

        public void CreateItem(Item itemToCreate)
        {
            using (var dbContextScope = _dbContextScopeFactory.Create())
            {
                Item foundItem = _itemRepository.FindById(itemToCreate.Id);
                if (foundItem != null)
                    throw new Exception(); //TODO: item already exists
                
                _itemRepository.Add(itemToCreate);
                dbContextScope.SaveChanges();
            }
        }

        public void DeleteItem(int itemId)
        {
            using (var dbContextScope = _dbContextScopeFactory.Create())
            {

                var foundItem = _itemRepository.FindById(itemId);
                if (foundItem == null)
                {
                    //TODO: ItemNotFoundException
                    throw new Exception();
                }

                _itemRepository.Remove(foundItem);
                dbContextScope.SaveChanges();
            }
        }

        public void UpdateItem(Item itemToUpdate)
        {
            if (itemToUpdate == null)
                throw new ArgumentNullException("itemToUpdate");

            using (var dbContextScope = _dbContextScopeFactory.Create())
            {

                var foundItem = _itemRepository.FindById(itemToUpdate.Id);
                if (foundItem == null)
                {
                    //TODO: CategoryNotFoundException
                    throw new Exception();
                }
                //TODO: copy everything here or Attach from DbContext
                foundItem.Name = itemToUpdate.Name;
                foundItem.Description = itemToUpdate.Description;
                foundItem.Price = itemToUpdate.Price;
                if (itemToUpdate.CategoryId != foundItem.CategoryId)
                {
                    foundItem.Category = itemToUpdate.Category;
                    foundItem.CategoryId = itemToUpdate.CategoryId;
                }
                foundItem.ItemProperties = itemToUpdate.ItemProperties;
                _itemRepository.Modify(foundItem);
                dbContextScope.SaveChanges();
            }
        }

        public void UpdateItemImage(Item itemToUpdate, string folderToImage)
        {
            if (itemToUpdate == null)
                throw new ArgumentNullException("itemToUpdate");

            using (var dbContextScope = _dbContextScopeFactory.Create())
            {

                var foundItem = _itemRepository.FindById(itemToUpdate.Id);
                if (foundItem == null)
                {
                    //TODO: CategoryNotFoundException
                    throw new Exception();
                }
                try
                {
                    itemToUpdate.ImageUrl = CreateItemUrl(itemToUpdate, folderToImage);
                }
                catch
                {
                    throw;
                }
                //TODO: copy everything here or Attach from DbContext
                foundItem.ImageUrl = itemToUpdate.ImageUrl;
                foundItem.Image = itemToUpdate.Image;

                _itemRepository.Modify(foundItem);
                dbContextScope.SaveChanges();
            }
        }

        public void AddPropertyToItem(int itemId, int propertyId, string value)
        {
            using (var dbContextScope = _dbContextScopeFactory.Create())
            {
                var item = _itemRepository.FindById(itemId);
                var property = _propertyService.GetProperty(propertyId);

                if (item == null)
                    throw new ArgumentException($"Item with id {itemId} not found.");

                if (property == null)
                    throw new ArgumentException($"Property with id {propertyId} not found.");

                var itemProperty = new ItemProperty
                {
                    ItemId = itemId,
                    PropertyId = propertyId,
                    Value = value
                };

                item.ItemProperties.Add(itemProperty);
                dbContextScope.SaveChanges();
            }
        }

        public void DeleteItemProperty(int itemId, int propertyId)
        {
            using (var dbContextScope = _dbContextScopeFactory.Create())
            {
                var item = _itemRepository.FindById(itemId);
                var itemProperty = item.ItemProperties.SingleOrDefault(x => x.PropertyId == propertyId);

                if (item == null)
                    throw new ArgumentException($"Item with id {itemId} not found.");
                
                item.ItemProperties.Remove(itemProperty);
                dbContextScope.SaveChanges();
            }
        }
    }
}
