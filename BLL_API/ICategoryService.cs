using BOL;
using BOL.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_API
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAllCategories();
        Category GetCategory(int categoryId);
        void CreateCategory(Category categoryToCreate);
        void UpdateCategory(Category categoryToUpdate);
        void DeleteCategory(int categoryId);
    }
}
