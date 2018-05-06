using BOL.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
