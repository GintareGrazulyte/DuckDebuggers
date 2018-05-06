﻿using BOL.Accounts;
using BOL.Objects;
using System.Threading.Tasks;

namespace BLL_API
{
    public interface IItemManagementService
    {
        Task ImportItemsFromFile(string path, Admin admin);
        void ExportAllItemsToFile(string path);
        void CreateItemWithImage(Item itemToCreate, string folderToImage);
        void CreateItem(Item itemToCreate);
        void UpdateItem(Item itemToUpdate);
        void DeleteItem(int itemId);
    }
}
