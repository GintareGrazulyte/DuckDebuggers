using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DAL_API;
using BOL.Accounts;
using Mehdime.Entity;
using System;

namespace DAL
{
    public class AdminRepository : IAdminRepository
    {
        private readonly IAmbientDbContextLocator _ambientDbContextLocator;

        private EShopDbContext DbContext
        {
            get
            {
                var dbContext = _ambientDbContextLocator.Get<EShopDbContext>();

                if (dbContext == null)
                    throw new InvalidOperationException("No ambient DbContext of type EShopDbContext found");

                return dbContext;
            }
        }

        public AdminRepository(IAmbientDbContextLocator ambientDbContextLocator)
        {
            if (ambientDbContextLocator == null) throw new ArgumentNullException("ambientDbContextLocator");
            _ambientDbContextLocator = ambientDbContextLocator;
        }

        public Admin FindByEmail(string email)
        {
            return DbContext.Admins
                    .Where(c => c.Email == email)
                    .FirstOrDefault();
        }

        public void Modify(Admin admin)
        {
            DbContext.Entry(admin).State = EntityState.Modified;
        }

        public List<Admin> GetAll()
        {
            return DbContext.Admins.ToList();
        }
    }
}
