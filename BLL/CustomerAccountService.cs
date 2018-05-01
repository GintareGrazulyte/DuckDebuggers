using BLL_API;
using BOL.Accounts;
using BOL.Utils;
using DAL_API;
using Mehdime.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class CustomerAccountService : ICustomerAccountService
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly ICustomerRepository _customerRepository;

        public CustomerAccountService(IDbContextScopeFactory dbContextScopeFactory, ICustomerRepository customerRepository)
        {
            if (dbContextScopeFactory == null) throw new ArgumentNullException("dbContextScopeFactory");
            if (customerRepository == null) throw new ArgumentNullException("customerRepository");
            _dbContextScopeFactory = dbContextScopeFactory;
            _customerRepository = customerRepository;
        }

        public void CreateCustomer(Customer customerToCreate)
        {
            if (customerToCreate == null)
                throw new ArgumentNullException("userToCreate");

            using (var dbContextScope = _dbContextScopeFactory.Create())
            {

                var foundCustomer = _customerRepository.FindByEmail(customerToCreate.Email);
                if (foundCustomer != null)
                {
                    //TODO: UserAlreadyExistsException
                    throw new Exception();
                }

                customerToCreate.Password = Encryption.SHA256(customerToCreate.Password);
                customerToCreate.ConfirmPassword = Encryption.SHA256(customerToCreate.ConfirmPassword);
                customerToCreate.IsActive = true;

                _customerRepository.Add(customerToCreate);
                dbContextScope.SaveChanges();
            }
        }

        public Customer LoginCustomer(Customer customerToLogin)
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                var foundCustomer = _customerRepository.FindByEmail(customerToLogin.Email);


                if (foundCustomer != null 
                    && foundCustomer.Password == Encryption.SHA256(customerToLogin.Password) 
                    && foundCustomer.IsActive)
                {
                    return foundCustomer;
                }
                else    //TODO: WrongEmailOrPasswordException
                    return null;
            }
        }
    }
}