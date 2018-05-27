using BLL_API;
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
        public List<Property> GetAllProperties()
        {
            using (var dbContextScope = _dbContextScopeFactory.CreateReadOnly())
            {
                return _propertyRepository.GetAll();
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
    }
}
