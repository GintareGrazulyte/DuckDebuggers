using BOL.Accounts;
using System;
using System.Collections.Generic;

namespace DAL_API
{
    public interface IAdminRepository : IDisposable
    {
        //TODO should be IAdmin
        Admin FindByEmail(string email);
        void Modify(Admin admin);
        List<Admin> GetAll();
    }
}