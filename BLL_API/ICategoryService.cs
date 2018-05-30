using BOL;
using System.Collections.Generic;

namespace BLL_API
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAllCategories();
        Category GetCategory(int categoryId);
        Category GetCategory(string categoryName);
        void CreateCategory(string name, List<int> properties);
        void UpdateCategory(Category categoryToUpdate);
        void UpdateCategory(int id, string name, List<int> properties);
        void DeleteCategory(int categoryId);
    }
}
