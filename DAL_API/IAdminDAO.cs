using DOL.Accounts;
using System;
using System.Collections.Generic;

namespace DAL_API
{
    public interface IAdminDAO : IDisposable
    {
        //TODO should be IAdmin
        Admin FindByEmail(string email);
        void Modify(Admin admin);
    }
}