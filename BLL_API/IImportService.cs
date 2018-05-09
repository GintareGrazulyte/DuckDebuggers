using BOL.Objects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL_API
{
    //TODO: find proper name
    public interface IImportService
    {
        void SetDocument(string path);
        List<Item> ImportItemsFromFile();
        string ExportItemsToFile(IEnumerable<Item> items);
    }
}
