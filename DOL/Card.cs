﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BOL
{
    public class Card
    {
        //All the card data should be hashed
        [Required(ErrorMessage = "Card number is required!")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Incorrect card number")]
        public string Number { get; set; }
        [Required(ErrorMessage = "Card holder is required!")]
        public string Holder { get; set; }
        [Required(ErrorMessage = "Card expiration year is required!")]
        [Range(2017, 2099, ErrorMessage = "Incorrect card expiration year number!")]
        public int ExpYear { get; set; }
        [Range(1, 12, ErrorMessage = "Incorrect card expiration month number!")]
        [Required(ErrorMessage = "Card expiration month is required!")]
        public int ExpMonth { get; set; }
        [NotMapped]
        public string CVV { get; set; }
    }
}
