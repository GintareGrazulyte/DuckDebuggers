using BOL.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BOL.Orders;
using System.ComponentModel;

namespace BOL.Carts
{
    public class CartItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int BuyPrice { get; set; }
        public Cart Cart { get; set; }
        public Item Item { get; set; }
    }
}
