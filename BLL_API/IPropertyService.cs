using BOL.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_API
{
    public interface IPropertyService
    {
        List<Property> GetAllProperties();
        Property GetProperty(int propertyId);
        void Delete(int propertyId);
        void AddProperty(string name);
    }
}
