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

namespace BLL
{
    public class ItemManagementService : IItemManagementService
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly IItemRepository _itemRepository;
        private readonly IFileLoader _fileLoader;
        private readonly IImportService _importService;
        private readonly IEmailService _emailService;
        private readonly ICategoryService _categoryService;

        public ItemManagementService(IDbContextScopeFactory dbContextScopeFactory, IItemRepository itemRepository,
                                    IImportService importService, IFileLoader fileLoader, IEmailService emailService,
                                    ICategoryService categoryService)
        {
            _dbContextScopeFactory = dbContextScopeFactory ?? throw new ArgumentNullException("dbContextScopeFactory");
            _itemRepository = itemRepository ?? throw new ArgumentNullException("itemRepository");
            _fileLoader = fileLoader ?? throw new ArgumentNullException("fileLoader");
            _importService = importService ?? throw new ArgumentNullException("importService");
            _emailService = emailService ?? throw new ArgumentNullException("emailService");
            _categoryService = categoryService ?? throw new ArgumentNullException("categoryService");
        }

        public async Task ImportItemsFromFile(Admin admin, string folderToFile, HttpPostedFileBase file)
        {
            await Task.Run(() =>
            {
                var fileName = _fileLoader.Load(folderToFile, file);

                var items = _importService.ImportItemsFromFile(Path.Combine(folderToFile, fileName));

                var email = new Email()
                {
                    ToName = admin.Name,
                    ToAddress = admin.Email,
                    Subject = "Import",
                    Body = "",
                    AttachmentPath = ""
                };

                var categories = _categoryService.GetAllCategories();

                using (var dbContextScope = _dbContextScopeFactory.Create())
                {
                    int addedCount = 0;
                    for (int i = 0; i < items.Count; i++)
                    {
                        if (!categories.Any(c => c.Id == items[i].CategoryId))
                        {
                            email.Body += i + 2 + ". <" + items[i].Name + "> is not added as category <" + 
                                items[i].CategoryId + "> does not exist" + Environment.NewLine;
                            continue;
                        }

                        _itemRepository.Add(items[i]);
                        addedCount++;
                    }
                    email.Body += addedCount + "/" + items.Count + " items were successfully added";

                    try
                    {
                        dbContextScope.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        email.Body = "Unable to save items to database. Exception message: " + e.Message;
                    }

                    _emailService.SendEmail(email);
                }
            });
        }

        public async Task ExportAllItemsToFile(Admin admin, IEnumerable<Item> items)
        {
            await Task.Run(() =>
            {
                var attachmentPath = _importService.ExportItemsToFile(items);
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

                //TODO: when temp file should be deleted?
                try
                {
                    File.Delete(attachmentPath);
                }
                catch (IOException)
                {
                    //file is in use or does not exist
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

                itemToCreate.ImageUrl = _fileLoader.Load(folderToImage, itemToCreate.Image);
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
                foundItem.ImageUrl = itemToUpdate.ImageUrl;
                foundItem.Image = itemToUpdate.Image;
                foundItem.Category = itemToUpdate.Category;
                foundItem.CategoryId = itemToUpdate.CategoryId;

                _itemRepository.Modify(foundItem);
                dbContextScope.SaveChanges();
            }
        }
    }
}
