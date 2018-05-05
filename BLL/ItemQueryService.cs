using BLL_API;
using BOL;
using BOL.Objects;
using DAL_API;
using Mehdime.Entity;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class ItemQueryService : IItemQueryService
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly IItemRepository _itemRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ItemQueryService(IDbContextScopeFactory dbContextScopeFactory, IItemRepository itemRepository,
            ICategoryRepository categoryRepository)
        {
            _dbContextScopeFactory = dbContextScopeFactory ?? throw new ArgumentNullException("dbContextScopeFactory");
            _itemRepository = itemRepository ?? throw new ArgumentNullException("itemRepository");
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException("categoryRepository");
        }

        public IEnumerable<Item> GetAllItems()
        {
            using (var dbContextScope = _dbContextScopeFactory.CreateReadOnly())
            {
                return _itemRepository.GetAll();
            }
        }

        public Item GetItem(int itemId)
        {
            using (var dbContextScope = _dbContextScopeFactory.CreateReadOnly())
            {
                Item item = _itemRepository.FindById(itemId);

                if (item == null)
                    throw new ArgumentException(String.Format("Invalid value provided for itemId: [{0}].", itemId));

                return item;
            }
        }

        public IEnumerable<Item> GetItems(int categoryId)
        {
            using (var dbContextScope = _dbContextScopeFactory.CreateReadOnly())
            {
                Category category = _categoryRepository.FindById(categoryId);

                if (category == null)
                    throw new ArgumentException(String.Format("Invalid value provided for categoryId: [{0}].", categoryId));

                return category.Items;
            }
        }
    }
}
