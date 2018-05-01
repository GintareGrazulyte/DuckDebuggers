using BOL;
using System;
using System.Collections.Generic;

namespace DAL_API
{
    public interface ICategoryRepository
    {
        Category FindById(int? id);
        Category FindByName(string name);
        List<Category> GetAll();
        void Remove(Category category);
        void Add(Category category);
        void Modify(Category category);
    }
}
