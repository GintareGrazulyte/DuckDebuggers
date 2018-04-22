using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DOL
{
    public class Card
    {
        //All the card data should be hashed
        [Required(ErrorMessage = "Card number is required!")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Incorrect card number")]
        public string Number { get; set; }
        [Required(ErrorMessage = "Card holder is required!")]
        public string Holder { get; set; }
        [Required(ErrorMessage = "Card experation year is required!")]
        [Range(2017, 2099, ErrorMessage = "Incorrect card experation year number!")]
        public int ExpYear { get; set; }
        [Range(1, 12, ErrorMessage = "Incorrect card experation month number!")]
        [Required(ErrorMessage = "Card experation month is required!")]
        public int ExpMonth { get; set; }
        [Required(ErrorMessage = "Card cvv number is required!")]
        [Range(0, 999, ErrorMessage = "Incorrect card cvv number!")]
        public int CV { get; set; }
    }
}
