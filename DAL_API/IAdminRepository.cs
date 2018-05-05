using BOL.Accounts;
using System.Collections.Generic;

namespace DAL_API
{
    public interface IAdminRepository
    {
        //TODO should be IAdmin
        Admin FindByEmail(string email);
        Admin FindById(int id);
        void Modify(Admin admin);
        List<Admin> GetAll();
    }
}