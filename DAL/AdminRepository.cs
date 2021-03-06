﻿using BOL.Accounts;
using DAL_API;
using Mehdime.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

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
            _ambientDbContextLocator = ambientDbContextLocator ?? throw new ArgumentNullException("ambientDbContextLocator");
        }

        public Admin FindByEmail(string email)
        {
            return DbContext.Admins
                    .Where(c => c.Email == email)
                    .FirstOrDefault();
        }

        public Admin FindById(int id)
        {
            return DbContext.Admins
                    .Where(c => c.Id == id)
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

        public void Add(Admin admin)
        {
                DbContext.Admins.Add(admin);
        }
    }
}
