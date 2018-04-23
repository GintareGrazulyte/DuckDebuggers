using System.ComponentModel.DataAnnotations;

namespace DOL
{
    public class DeliveryAddress
    {
        [Required(ErrorMessage = "Street number is required!")]
        public int StreetNumber { get; set; }
        [Required(ErrorMessage = "Country is required!")]
        public string Country { get; set; }
        [Required(ErrorMessage = "Street is required!")]
        public string Street { get; set; }
        [Required(ErrorMessage = "City is required!")]
        public string City { get; set; }
        [Required(ErrorMessage = "Apartment number is required!")]
        public int Apartment { get; set; }
    }
}
