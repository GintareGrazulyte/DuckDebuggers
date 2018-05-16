using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EShop.Models
{
    public class UserViewModel
    {
        public List<User> FoundCustomers { get; set; }
        public List<User> AllCUstomers { get; set; }
        public List<User> AllAdmins { get; set; }

        public class User
        {
            [Required]
            public int Id { get; set; }
            [Required]
            [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3]\.)|(([\w-]+\.)+))([a-zA-Z{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter valid email.")]
            public string Email { get; set; }
            [Required]
            public string Name { get; set; }
            [Required]
            public string Surname { get; set; }
            public bool IsActive { get; set; }
        }
    }
}