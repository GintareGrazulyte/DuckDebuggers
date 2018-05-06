﻿using BLL_API;
using BOL;
using BOL.Accounts;
using BOL.Objects;
using DAL_API;
using Mehdime.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL
{
    public class ItemManagementService : IItemManagementService
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly IItemRepository _itemRepository;
        private readonly IFileLoader _fileLoader;
        private readonly IImportService _importService;
        private readonly IEmailService _emailService;

        public ItemManagementService(IDbContextScopeFactory dbContextScopeFactory, IItemRepository itemRepository,
                                    IImportService importService, IFileLoader fileLoader, IEmailService emailService)
        {
            _dbContextScopeFactory = dbContextScopeFactory ?? throw new ArgumentNullException("dbContextScopeFactory");
            _itemRepository = itemRepository ?? throw new ArgumentNullException("itemRepository");
            _fileLoader = fileLoader ?? throw new ArgumentNullException("fileLoader");
            _importService = importService ?? throw new ArgumentNullException("importService");
            _emailService = emailService ?? throw new ArgumentNullException("emailService");
        }

        public Task ImportItemsFromFile(string path, Admin admin)
        {
            return _importService.ImportItemsFromFile(path).ContinueWith((items) =>
            {
                using (var dbContextScope = _dbContextScopeFactory.Create())
                {
                    foreach (var item in items.Result)
                        _itemRepository.Add(item);

                    dbContextScope.SaveChanges();
                }

                var email = new Email()
                                {
                                    ToName = admin.Name,
                                    ToAddress = admin.Email,
                                    Subject = "Import",
                                    Body = "Items import completed successfully"
                                };

                _emailService.SendEmail(email);
            });       
        }

        public void ExportAllItemsToFile(string path)
        {
            using (var dbContextScope = _dbContextScopeFactory.CreateReadOnly())
            {
                IEnumerable<Item> items = _itemRepository.GetAll();
                _importService.ExportItemsToFile(items, path);
            }   
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
