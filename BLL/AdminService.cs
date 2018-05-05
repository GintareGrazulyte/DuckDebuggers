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
            if (dbContextScopeFactory == null) throw new ArgumentNullException("dbContextScopeFactory");
            if (adminRepository == null) throw new ArgumentNullException("adminRepository");
            if (customerRepository == null) throw new ArgumentNullException("customerRepository");
            _dbContextScopeFactory = dbContextScopeFactory;
            _adminRepository = adminRepository;
            _customerRepository = customerRepository;
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

        public List<Customer> GetCustomers()
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                return _customerRepository.GetAll();
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

        public Customer GetCustomer(int id)
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                return _customerRepository.FindById(id);
            }
        }

        public void ChangeStatus(Customer account)
        {
            if (account == null)
                throw new ArgumentNullException("accountStatusToChange");

            using (var dbContextScope = _dbContextScopeFactory.Create())
            {
                var foundAccount = _customerRepository.FindById(account.Id);
                foundAccount.IsActive = !account.IsActive;
                _customerRepository.Modify(foundAccount);
                dbContextScope.SaveChanges();
            }
        }
    }
}
