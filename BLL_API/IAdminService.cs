using BOL.Accounts;
using System.Collections.Generic;

namespace BLL_API
{
    public interface IAdminService
    {
        Admin LoginAdmin(Admin adminToLogin);
        Admin GetAdmin(string email);
        Admin GetAdmin(int id);
        List<Admin> GetAdmins();
        void CreateAdmin(Admin adminToCreate);
    }
}
