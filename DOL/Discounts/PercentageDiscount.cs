using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL.Discounts
{
    public class PercentageDiscount : Discount
    {
        public decimal Percentage { get; set; }

        public override decimal CalculateDiscountedPrice(int itemPrice)
        {
            return itemPrice * Percentage / 100;
        }
    }
}
