﻿using BOL.Property;
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
        List<Property> GetProperties(List<int> ids);
        Property GetProperty(int propertyId);
        Property GetProperty(string name);
        void Update(Property property);
        void Delete(int propertyId);
        void AddProperty(string name);
    }
}
