using System.Data.Entity;
using System.Linq;
using DAL_API;
using DOL.Accounts;

namespace DAL
{
    public class AdminDAO : IAdminDAO
    {
        private EShopDbContext _db = new EShopDbContext();

        public void Dispose()
        {
            _db.Dispose();
        }

        public Admin FindByEmail(string email)
        {
            return _db.Admins
                    .Where(c => c.Email == email)
                    .FirstOrDefault();
        }

        public void Modify(Admin admin)
        {
            _db.Entry(admin).State = EntityState.Modified;
            _db.SaveChanges();
        }
    }
}
