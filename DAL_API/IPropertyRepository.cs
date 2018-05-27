using BOL.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_API
{
    public interface IPropertyRepository
    {
        Property FindById(int id);
        List<Property> GetAll();
        void Remove(Property property);
        void Add(Property property);
        void Modify(Property property);
    }
}
