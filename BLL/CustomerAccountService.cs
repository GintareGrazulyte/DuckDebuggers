using BLL_API;
using BOL.Accounts;
using DAL_API;
using Mehdime.Entity;
using System;
using System.Collections.Generic;

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
            {
                throw new ArgumentNullException("userToCreate");
            }

            if (!customerToCreate.IsConfirmPasswordCorrect())
            {
                throw new Exception("Password doesn't match");
            }

            using (var dbContextScope = _dbContextScopeFactory.Create())
            {

                var foundCustomer = _customerRepository.FindByEmail(customerToCreate.Email);
                if (foundCustomer != null)
                {
                    //TODO: UserAlreadyExistsException
                    throw new Exception("User already exists");
                }

                customerToCreate.HashPassword();
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
                    && foundCustomer.IsCorrectPassword(customerToLogin.Password)
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

        public List<Customer> GetCustomers()
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                return _customerRepository.GetAll();
            }
        }

        public void Modify(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customerToUpdate");

            using (var dbContextScope = _dbContextScopeFactory.Create())
            {

                var foundCustomer = _customerRepository.FindById(customer.Id);
                if (foundCustomer == null)
                {
                    //TODO: CategoryNotFoundException
                    throw new Exception();
                }

                //TODO: copy everything here or Attach from DbContext
                foundCustomer.Email = customer.Email;
                foundCustomer.Name = customer.Name;
                foundCustomer.Surname = customer.Surname;
                foundCustomer.Card = customer.Card;
                foundCustomer.DeliveryAddress = customer.DeliveryAddress;

                _customerRepository.Modify(foundCustomer);
                dbContextScope.SaveChanges();
            }
        }

        public void UpdatePassword(int customerId, string newPassword)
        {
            using (var dbContextScope = _dbContextScopeFactory.Create())
            {

                var foundCustomer = _customerRepository.FindById(customerId);
                if (foundCustomer == null)
                {
                    //TODO: CategoryNotFoundException
                    throw new Exception();
                }

                //TODO: copy everything here or Attach from DbContext
                foundCustomer.Password = newPassword;
                foundCustomer.HashPassword();

                _customerRepository.Modify(foundCustomer);
                dbContextScope.SaveChanges();
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