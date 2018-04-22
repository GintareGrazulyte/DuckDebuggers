using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DOL.Orders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DOL.Accounts
{
    public class Customer : Account
    {
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords don't match!")]
        public string ConfirmPassword { get; set; }
        public Card Card { get; set; }
        public DeliveryAddress DeliveryAddress { get; set; }
        public ICollection<Order> Order { get; set; }
    }
}
