using BOL.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_API
{
    public interface IItemQueryService
    {
        Item GetItem(int itemId);
        IEnumerable<Item> GetItems(int categoryId);
    }
}
