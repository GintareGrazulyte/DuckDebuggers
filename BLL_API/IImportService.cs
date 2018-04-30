using DOL.Objects;
using System.Collections.Generic;

namespace BLL_API
{
    //TODO: find proper name
    public interface IImportService
    {
        ICollection<Item> GetItemsFromFile(string path);
    }
}
