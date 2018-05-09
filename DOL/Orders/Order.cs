using BOL.Carts;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BOL.Orders
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Cart Cart { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime DateTime { get; set; }
    }
}
