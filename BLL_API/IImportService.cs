using BOL.Objects;
using System.Collections.Generic;

namespace BLL_API
{
    //TODO: find proper name
    public interface IImportService
    {
        List<Item> ImportItemsFromFile(string path);
        string ExportItemsToFile(IEnumerable<Item> items, string folderToSave);
    }
}
