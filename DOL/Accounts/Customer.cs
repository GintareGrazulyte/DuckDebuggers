using System.Collections.Generic;
using BOL.Orders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BOL.Utils;

namespace BOL.Accounts
{
    public class Customer : Account
    {
        [DataType(DataType.Password)]
        [NotMapped]
        public string ConfirmPassword { get; set; }
        public Card Card { get; set; }
        public DeliveryAddress DeliveryAddress { get; set; }
        public ICollection<Order> Orders { get; set; }

        public bool IsConfirmPasswordCorrect()
        {
            return Password == ConfirmPassword;
        }

        public bool IsCorrectPassword(string unhashedPassword)
        {
            return Password == Encryption.SHA256(unhashedPassword);
        }

        public void HashPassword()
        {
            Password = Encryption.SHA256(Password);
        }
    }
}
