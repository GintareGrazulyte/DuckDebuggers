using BOL.Objects;
using System.Collections.Generic;

namespace BLL_API
{
    //TODO: find proper name
    public interface IImportService
    {
        ICollection<Item> ImportItemsFromFile(string path);
        void ExportItemsToFile(ICollection<Item> items, string path);
    }
}
