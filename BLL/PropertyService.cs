﻿using BLL_API;
using BOL.Property;
using DAL_API;
using Mehdime.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class PropertyService : IPropertyService
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly IPropertyRepository _propertyRepository;

        public PropertyService(IDbContextScopeFactory dbContextScopeFactory, IPropertyRepository propertyRepository)
        {
            _dbContextScopeFactory = dbContextScopeFactory ?? throw new ArgumentNullException("dbContextScopeFactory");
            _propertyRepository = propertyRepository ?? throw new ArgumentNullException("propertyRepository");
        }

        public void AddProperty(string name)
        {
            using (var dbContextScope = _dbContextScopeFactory.Create())
            {
                var property = new Property
                {
                    Name = name
                };

                _propertyRepository.Add(property);
                dbContextScope.SaveChanges();
            }
        }

        public void Delete(int propertyId)
        {
            using (var dbContextScope = _dbContextScopeFactory.Create())
            {
                var property = _propertyRepository.FindById(propertyId);

                if (property == null)
                    throw new ArgumentException($"Property with id {propertyId} not found");

                _propertyRepository.Remove(property);
                dbContextScope.SaveChanges();
            }
        }

        public List<Property> GetAllProperties()
        {
            using (var dbContextScope = _dbContextScopeFactory.CreateReadOnly())
            {
                return _propertyRepository.GetAll();
            }
        }

        public List<Property> GetProperties(List<int> ids)
        {
            using (var dbContextScope = _dbContextScopeFactory.CreateReadOnly())
            {
                return _propertyRepository.GetByIds(ids);
            }
        }

        public Property GetProperty(int propertyId)
        {
            using (var dbContextScope = _dbContextScopeFactory.CreateReadOnly())
            {
                var property = _propertyRepository.FindById(propertyId);

                if (property == null)
                    throw new ArgumentException($"Property with id {propertyId} not found");

                return property;
            }
        }

        public Property GetProperty(string propertyName)
        {
            using (var dbContextScope = _dbContextScopeFactory.CreateReadOnly())
            {
                var property = _propertyRepository.FindByName(propertyName);

                return property;
            }
        }

        public void Update(Property property)
        {
            if (property == null)
                throw new ArgumentNullException("itemToUpdate");

            using (var dbContextScope = _dbContextScopeFactory.Create())
            {

                var foundProperty = _propertyRepository.FindById(property.Id);
                if (foundProperty == null)
                {
                    throw new Exception();
                }

                foundProperty.Name = property.Name;

                _propertyRepository.Modify(foundProperty);
                dbContextScope.SaveChanges();
            }
        }
    }
}
