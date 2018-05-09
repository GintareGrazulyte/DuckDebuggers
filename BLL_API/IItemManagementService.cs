using BOL.Accounts;
using BOL.Objects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL_API
{
    public interface IItemManagementService
    {
        void SetDocument(string path);
        Task ImportItemsFromFile(Admin admin);
        Task ExportAllItemsToFile(Admin admin, IEnumerable<Item> allItems);
        void CreateItemWithImage(Item itemToCreate, string folderToImage);
        void CreateItem(Item itemToCreate);
        void UpdateItem(Item itemToUpdate);
        void DeleteItem(int itemId);
    }
}
