using BOL.Objects;
using System.Collections.Generic;

namespace BLL_API
{
    //TODO: find proper name
    public interface IImportService
    {
        IEnumerable<Item> ImportItemsFromFile(string path);
        void ExportItemsToFile(IEnumerable<Item> items, string path);
    }
}
