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
        Property FindByName(string name);
        List<Property> GetAll();
        List<Property> GetByIds(List<int> ids);
        void Remove(Property property);
        void Add(Property property);
        void Modify(Property property);
    }
}
