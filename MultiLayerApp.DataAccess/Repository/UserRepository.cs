using MultiLayerApp.DataAccess.Data;
using MultiLayerApp.DataAccess.Repository.IRepository;
using System.Linq;
using MultiLayerApp.DataAccess.Domain;

namespace MultiLayerApp.DataAccess.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly ShopDbContext _db;

        public UserRepository(ShopDbContext db) : base(db)
        {
            _db = db;
        }

    }
}