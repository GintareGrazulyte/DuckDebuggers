using BOL.Objects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL_API
{
    //TODO: find proper name
    public interface IImportService
    {
        Task<List<Item>> ImportItemsFromFile(string path);
        void ExportItemsToFile(IEnumerable<Item> items, string path);
    }
}
