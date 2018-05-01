using BLL_API;
using BOL.Accounts;
using DAL_API;
using Mehdime.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class AdminService : IAdminService
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly IAdminRepository _adminRepository;

        public AdminService(IDbContextScopeFactory dbContextScopeFactory, IAdminRepository adminRepository)
        {
            if (dbContextScopeFactory == null) throw new ArgumentNullException("dbContextScopeFactory");
            if (adminRepository == null) throw new ArgumentNullException("adminRepository");
            _dbContextScopeFactory = dbContextScopeFactory;
            _adminRepository = adminRepository;
        }

        public Admin GetAdmin(string email)
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                return _adminRepository.FindByEmail(email);
            }
        }

        public Admin LoginAdmin(Admin adminToLogin)
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                var foundAdmin = _adminRepository.FindByEmail(adminToLogin.Email);
                //TODO needs to hashed, but how do you create an admin then?
                if (foundAdmin != null && foundAdmin.Password == adminToLogin.Password)
                {
                    return foundAdmin;
                }
                else    //TODO: WrongEmailOrPasswordException
                    return null;
            }
        }
    }
}
