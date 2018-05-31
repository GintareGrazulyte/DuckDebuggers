using BOL.Discounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_API
{
    public interface IDiscountManagementService
    {
        void CreateDiscount(Discount discount, IEnumerable<int> itemIds);
        List<Discount> GetAllDiscounts();
        Discount GetDiscount(int id);
        void DeleteExpiredDiscounts();
        void DeleteDiscount(int discountId);
    }
}
