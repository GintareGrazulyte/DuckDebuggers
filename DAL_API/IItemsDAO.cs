using DOL.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_API
{
    public interface IItemsDAO : IDisposable
    {
        Item Find(int? id);
        List<Item> GetAll();
        void Remove(Item item);
        void Add(Item item);
        void Modify(Item item);
    }
}
