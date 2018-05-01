using BLL_API;
using BOL;
using BOL.Objects;
using DAL_API;
using Mehdime.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class CategoryService : ICategoryService
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(IDbContextScopeFactory dbContextScopeFactory, ICategoryRepository categoryRepository)
        {
            if (dbContextScopeFactory == null) throw new ArgumentNullException("dbContextScopeFactory");
            if (categoryRepository == null) throw new ArgumentNullException("categoryRepository");
            _dbContextScopeFactory = dbContextScopeFactory;
            _categoryRepository = categoryRepository;
        }

        public void CreateCategory(Category categoryToCreate)
        {
            if (categoryToCreate == null)
                throw new ArgumentNullException("categoryToCreate");

            using (var dbContextScope = _dbContextScopeFactory.Create())
            {

                var foundCategory = _categoryRepository.FindById(categoryToCreate.Id);
                if (foundCategory != null)
                {
                    //TODO: CategoryAlreadyExistsException
                    throw new Exception();
                }

                _categoryRepository.Add(categoryToCreate);
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

                //TODO: copy everything here or Attach from DbContext
                foundCategory.Name = categoryToUpdate.Name;

                _categoryRepository.Modify(foundCategory);
                dbContextScope.SaveChanges();
            }
        }
    }
}
