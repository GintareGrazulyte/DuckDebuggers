using BOL.Orders;
using System.Collections.Generic;

namespace DAL_API
{
    public interface IOrderRatingRepository
    {
        OrderRating FindById(int? id);
        OrderRating FindByOrderId(int? id);
        void Remove(OrderRating orderRating);
        void Add(OrderRating orderRating);
        List<OrderRating> GetAll();
    }
}
