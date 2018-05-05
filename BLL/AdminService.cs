using BLL_API;
using BOL.Accounts;
using DAL_API;
using Mehdime.Entity;
using System;

namespace BLL
{
    public class AdminService : IAdminService
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly IAdminRepository _adminRepository;

        public AdminService(IDbContextScopeFactory dbContextScopeFactory, IAdminRepository adminRepository)
        {
            _dbContextScopeFactory = dbContextScopeFactory ?? throw new ArgumentNullException("dbContextScopeFactory");
            _adminRepository = adminRepository ?? throw new ArgumentNullException("adminRepository");
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
