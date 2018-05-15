using BOL.Orders;
using System.Collections.Generic;

namespace DAL_API
{
    public interface IOrderRepository
    {
        Order FindById(int? id);
        List<Order> GetAll();
        void Modify(Order order);
    }
}
