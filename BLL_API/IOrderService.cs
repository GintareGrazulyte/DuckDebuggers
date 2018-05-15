using BOL.Orders;
using System.Collections.Generic;

namespace BLL_API
{
    public interface IOrderService
    {
        IEnumerable<Order> GetAllOrders();
        Order GetOrder(int orderId);
        void UpdateStatus(int orderId, OrderStatus status);
    }
}
