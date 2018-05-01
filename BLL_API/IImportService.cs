using DOL.Objects;
using System.Collections.Generic;

namespace BLL_API
{
    public interface IImportService
    {
        ICollection<Item> GetItemsFromFile(string path);
    }
}
