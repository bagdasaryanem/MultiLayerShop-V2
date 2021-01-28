using MultiLayerApp.DataAccess.Data;
using MultiLayerApp.DataAccess.Repository.IRepository;

namespace MultiLayerApp.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ShopDbContext _db;

        public UnitOfWork(ShopDbContext db, ICategoryRepository categoryRepository, IProductRepository productRepository, 
            IUserRepository userRepository, IShoppingCartRepository shoppingCartRepository, IOrderHeaderRepository orderHeaderRepository)
        {
            _db = db;
            Category = categoryRepository;
            Product = productRepository;
            User = userRepository;
            OrderHeader = orderHeaderRepository;
            ShoppingCart = shoppingCartRepository;
        }

        public IUserRepository User { get; private set; }
        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IOrderHeaderRepository OrderHeader { get; private set; }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}