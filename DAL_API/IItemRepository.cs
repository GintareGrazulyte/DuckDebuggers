using BOL.Objects;
using System.Collections.Generic;

namespace DAL_API
{
    public interface IItemRepository
    {
        Item FindById(int? id);
        List<Item> GetAll();
        void Remove(Item item);
        void Add(Item item);
        void Modify(Item item);
        List<Item> GetByIds(IEnumerable<int> ids);
    }
}
