using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL.Discounts
{
    public class AbsoluteDiscount : Discount
    {
        public int AbsoluteValue { get; set; }

        public override decimal CalculateDiscountedPrice(int itemPrice)
        {
            /*if (itemPrice - AbsoluteValue < 0) throw new ArgumentException($"Negative price exception. " +
                                                                        $"Can't apply absolute discount: {AbsoluteValue} " +
                                                                        $"to item with price: {itemPrice}");*/
            var value = itemPrice - AbsoluteValue;
            return value < 0 ? 0 : value;
        }
    }
}
