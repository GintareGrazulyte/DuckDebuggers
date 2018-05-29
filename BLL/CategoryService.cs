using BLL_API;
using BOL;
using DAL_API;
using Mehdime.Entity;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class CategoryService : ICategoryService
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPropertyService _propertyService;

        public CategoryService(IDbContextScopeFactory dbContextScopeFactory, ICategoryRepository categoryRepository,
                                    IPropertyService propertyService)
        {
            _dbContextScopeFactory = dbContextScopeFactory ?? throw new ArgumentNullException("dbContextScopeFactory");
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException("categoryRepository");
            _propertyService = propertyService ?? throw new ArgumentNullException("propertyService");
        }

        public void CreateCategory(string name, List<int> propertiesIds)
        {
            using (var dbContextScope = _dbContextScopeFactory.Create())
            {
                var foundCategoryByName = _categoryRepository.FindByName(name);
                if (foundCategoryByName != null)
                {
                    throw new Exception("Category already exists");
                }
                var properties = _propertyService.GetProperties(propertiesIds);
                var category = new Category { Name = name, Properties = properties};
                _categoryRepository.Add(category);
                dbContextScope.SaveChanges();
            }
        }

        public void DeleteCategory(int categoryId)
        {
            using (var dbContextScope = _dbContextScopeFactory.Create())
            {

                var foundCategory = _categoryRepository.FindById(categoryId);
                if (foundCategory == null)
                {
                    //TODO: CategoryNotFoundException
                    throw new Exception();
                }

                _categoryRepository.Remove(foundCategory);
                dbContextScope.SaveChanges();
            }
        }

        public IEnumerable<Category> GetAllCategories()
        {
            using (var dbContextScope = _dbContextScopeFactory.CreateReadOnly())
            {
                return _categoryRepository.GetAll();
            }
        }

        public Category GetCategory(int categoryId)
        {            
            using (var dbContextScope = _dbContextScopeFactory.CreateReadOnly())
            {
                Category category = _categoryRepository.FindById(categoryId);

                if (category == null)
                    throw new ArgumentException(String.Format("Invalid value provided for categoryId: [{0}].", categoryId));

                return category;
            }
        }

        public Category GetCategory(string categoryName)
        {
            using (var dbContextScope = _dbContextScopeFactory.CreateReadOnly())
            {
                Category category = _categoryRepository.FindByName(categoryName);

                if (category == null)
                    throw new ArgumentException(String.Format("Invalid value provided for category name: [{0}].", categoryName));

                return category;
            }
        }

        public void UpdateCategory(Category categoryToUpdate)
        {
            if (categoryToUpdate == null)
                throw new ArgumentNullException("categoryToCreate");

            using (var dbContextScope = _dbContextScopeFactory.Create())
            {

                var foundCategory = _categoryRepository.FindById(categoryToUpdate.Id);
                if (foundCategory == null)
                {
                    //TODO: CategoryNotFoundException
                    throw new Exception();
                }

                var foundCategoryByName = _categoryRepository.FindByName(categoryToUpdate.Name);
                if (foundCategoryByName != null)
                {
                    throw new Exception("Category already exists");
                }

                //TODO: copy everything here or Attach from DbContext
                foundCategory.Name = categoryToUpdate.Name;

                _categoryRepository.Modify(foundCategory);
                dbContextScope.SaveChanges();
            }
        }
    }
}
