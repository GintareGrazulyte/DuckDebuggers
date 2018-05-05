using BLL_API;
using BOL.Accounts;
using BOL.Utils;
using DAL_API;
using Mehdime.Entity;
using System;

namespace BLL
{
    public class CustomerAccountService : ICustomerAccountService
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly ICustomerRepository _customerRepository;

        public CustomerAccountService(IDbContextScopeFactory dbContextScopeFactory, ICustomerRepository customerRepository)
        {
            _dbContextScopeFactory = dbContextScopeFactory ?? throw new ArgumentNullException("dbContextScopeFactory");
            _customerRepository = customerRepository ?? throw new ArgumentNullException("customerRepository");
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

        public Customer GetCustomer(int customerId)
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                return _customerRepository.FindById(customerId);
            }
        }
    }
}