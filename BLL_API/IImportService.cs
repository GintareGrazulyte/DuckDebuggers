using BOL.Objects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL_API
{
    //TODO: find proper name
    public interface IImportService
    {
        List<Item> ImportItemsFromFile(string path);
        string ExportItemsToFile(IEnumerable<Item> items);
    }
}
