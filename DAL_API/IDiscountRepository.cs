using BOL.Discounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_API
{
    public interface IDiscountRepository
    {
        Discount FindById(int? id);
        List<Discount> GetAll();
        void Remove(Discount discount);
        void Add(Discount discount);
        void Modify(Discount discount);
    }
}
