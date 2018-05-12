using BOL.Accounts;
using System.Collections.Generic;

namespace BLL_API
{
    public interface IAdminService
    {
        Admin LoginAdmin(Admin adminToLogin);
        Admin GetAdmin(string email);
        Admin GetAdmin(int id);
        Customer GetCustomer(int id);
        List<Customer> GetCustomers();
        List<Customer> GetCustomers(string keyWord);
        List<Admin> GetAdmins();
        void ChangeStatus(Customer account);
        void CreateAdmin(Admin adminToCreate);
    }
}
