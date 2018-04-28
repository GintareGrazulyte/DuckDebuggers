using DOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_API
{
    public interface ICategoryDAO : IDisposable
    {
        Category FindById(int? id);
        List<Category> GetAll();
        void Remove(Category category);
        void Add(Category category);
        void Modify(Category category);
    }
}
