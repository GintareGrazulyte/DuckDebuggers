using BOL.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BOL.Accounts
{
    public abstract class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3]\.)|(([\w-]+\.)+))([a-zA-Z{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter valid email.")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public bool IsActive { get; set; }
        [DataType(DataType.Password)]
        [NotMapped]
        public string ConfirmPassword { get; set; }

        public bool IsConfirmPasswordCorrect()
        {
            return Password == ConfirmPassword;
        }

        public bool IsCorrectPassword(string unhashedPassword)
        {
            return Password.ToLower() == Encryption.SHA256(unhashedPassword).ToLower();
        }

        public void HashPassword()
        {
            Password = Encryption.SHA256(Password);
        }
    }
}
