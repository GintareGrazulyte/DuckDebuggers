using BOL.Accounts;
using BOL.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_API
{
    public interface IOrderRatingService
    {
        IEnumerable<OrderRating> GetAllOrderRatings();
        OrderRating GetOrderRating(int orderRatingId);
        OrderRating GetOrderRatingByOrderId(int orderId);
        void CreateOrderRating(OrderRating orderRatingToCreate);
        void DeleteOrderRating(int orderRatingtoDelete);
    }
}
