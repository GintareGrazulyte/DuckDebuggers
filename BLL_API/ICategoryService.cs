using BOL;
using System.Collections.Generic;

namespace BLL_API
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAllCategories();
        Category GetCategory(int categoryId);
        Category GetCategory(string categoryName);
        void CreateCategory(Category categoryToCreate);
        void UpdateCategory(Category categoryToUpdate);
        void DeleteCategory(int categoryId);
    }
}
