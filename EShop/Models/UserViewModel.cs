using BOL.Accounts;
using System.Collections.Generic;

namespace EShop.Models
{
    public class UserViewModel
    {
        public List<Customer> FoundCustomers { get; set; }
        public List<Customer> AllCUstomers { get; set; }
        public List<Admin> AllAdmins { get; set; }
    }
}