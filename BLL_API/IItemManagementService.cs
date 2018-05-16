using BOL.Accounts;
using BOL.Objects;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace BLL_API
{
    public interface IItemManagementService
    {
        Task ImportItemsFromFile(Admin admin, string folderToFile, HttpPostedFileBase file, string imagesFolder);
        Task ExportAllItemsToFile(Admin admin, IEnumerable<Item> allItems);
        void CreateItemWithImage(Item itemToCreate, string folderToImage);
        void CreateItem(Item itemToCreate);
        void UpdateItem(Item itemToUpdate);
        void DeleteItem(int itemId);
        void UpdateItemImage(Item itemToUpdate, string folderToImage);
    }
}
