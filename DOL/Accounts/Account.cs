using System;
using System.ComponentModel.DataAnnotations;


namespace DOL.Accounts
{
    public abstract class Account
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool IsActive { get; set; }
    }
}
