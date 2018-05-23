using BLL_API;
using BOL.Accounts;
using DAL_API;
using Mehdime.Entity;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class AdminService : IAdminService
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly IAdminRepository _adminRepository;
        private readonly ICustomerRepository _customerRepository;

        public AdminService(IDbContextScopeFactory dbContextScopeFactory, IAdminRepository adminRepository, ICustomerRepository customerRepository)
        {
            _dbContextScopeFactory = dbContextScopeFactory ?? throw new ArgumentNullException("dbContextScopeFactory");
            _adminRepository = adminRepository ?? throw new ArgumentNullException("adminRepository");
            _customerRepository = customerRepository ?? throw new ArgumentNullException("customerRepository");
        }

        public Admin GetAdmin(string email)
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                return _adminRepository.FindByEmail(email);
            }
        }

        public Admin GetAdmin(int id)
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                return _adminRepository.FindById(id);
            }
        }

        public List<Admin> GetAdmins()
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                return _adminRepository.GetAll();
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

        public List<Customer> GetCustomers(string keyWord)
        {
            return _customerRepository.GetAll(keyWord);
        }

        public void CreateAdmin(Admin adminToCreate)
        {
            if (adminToCreate == null)
            {
                throw new ArgumentNullException("userToCreate");
            }

            if (!adminToCreate.IsConfirmPasswordCorrect())
            {
                throw new Exception("Password doesn't match");
            }

            using (var dbContextScope = _dbContextScopeFactory.Create())
            {

                var foundCustomer = _adminRepository.FindByEmail(adminToCreate.Email);
                if (foundCustomer != null)
                {
                    //TODO: UserAlreadyExistsException
                    throw new Exception("User already exists");
                }

                adminToCreate.HashPassword();
                adminToCreate.IsActive = true;

                _adminRepository.Add(adminToCreate);
                dbContextScope.SaveChanges();
            }
        }
    }
}
