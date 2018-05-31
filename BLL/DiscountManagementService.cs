using BLL_API;
using BOL.Discounts;
using DAL_API;
using Mehdime.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class DiscountManagementService : IDiscountManagementService
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly IDiscountRepository _discountRepository;
        private readonly IItemRepository _itemRepository;

        public DiscountManagementService(IDbContextScopeFactory dbContextScopeFactory, 
                                            IDiscountRepository discountRepository,
                                                IItemRepository itemRepository)
        {
            _dbContextScopeFactory = dbContextScopeFactory ?? throw new ArgumentNullException("dbContextScopeFactory");
            _discountRepository = discountRepository ?? throw new ArgumentNullException("discountRepository");
            _itemRepository = itemRepository ?? throw new ArgumentNullException("itemRepository");
        }

        public void CreateDiscount(Discount discount, IEnumerable<int> itemIds)
        {
            using (var dbContextScope = _dbContextScopeFactory.Create())
            {

                var foundDiscount = _discountRepository.FindById(discount.Id);
                if (foundDiscount != null)
                {
                    throw new Exception("This discount already exists");
                }

                discount.Items = itemIds != null ? _itemRepository.GetByIds(itemIds) : null;

                _discountRepository.Add(discount);
                dbContextScope.SaveChanges();
            }
        }

        public void DeleteExpiredDiscounts()
        {
            using (var dbContextScope = _dbContextScopeFactory.Create())
            {
                var expiredDiscounts = _discountRepository.GetAll().Where(x => x.EndDate < DateTime.Now).ToArray();

                if (expiredDiscounts.Count() == 0)
                    return;

                foreach (var discount in expiredDiscounts)
                    _discountRepository.Remove(discount);

                dbContextScope.SaveChanges();
            }
        }

        public void DeleteDiscount(int discountId)
        {
            using (var dbContextScope = _dbContextScopeFactory.Create())
            {

                var foundDiscount = _discountRepository.FindById(discountId);
                if (foundDiscount == null)
                {
                    //TODO: DiscountNotFoundException
                    throw new Exception();
                }

                _discountRepository.Remove(foundDiscount);
                dbContextScope.SaveChanges();
            }
        }

        public List<Discount> GetAllDiscounts()
        {
            using (var dbContextScope = _dbContextScopeFactory.CreateReadOnly())
            {
                return _discountRepository.GetAll();
            }
        }

        public Discount GetDiscount(int id)
        {
            using (var dbContextScope = _dbContextScopeFactory.CreateReadOnly())
            {
                var foundDiscount = _discountRepository.FindById(id);
                if (foundDiscount == null)
                    throw new Exception($"Discount with id {id} not found.");

                return foundDiscount;
            }
        }
    }
}
