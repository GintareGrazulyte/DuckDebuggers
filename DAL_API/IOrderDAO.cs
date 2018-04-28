using DOL.Orders;
using System;
using System.Collections.Generic;

namespace DAL_API
{
    public interface IOrderDAO : IDisposable
    {
        Order Find(int? id);
        List<Order> GetAll();
        void Remove(Order item);
        void Add(Order item);
        void Modify(Order item);
    }
}
