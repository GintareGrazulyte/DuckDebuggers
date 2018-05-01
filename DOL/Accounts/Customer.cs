using System.Collections.Generic;
using BOL.Orders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BOL.Accounts
{
    public class Customer : Account
    {
        //TODO: error when modifying
      //  [Required]
        [DataType(DataType.Password)]
        [NotMapped]
       // [Compare("Password", ErrorMessage = "Passwords don't match!")]
        public string ConfirmPassword { get; set; }
        public Card Card { get; set; }
        public DeliveryAddress DeliveryAddress { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
