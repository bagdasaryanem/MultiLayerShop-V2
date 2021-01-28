using MultiLayerApp.DataAccess.Data;
using MultiLayerApp.DataAccess.Repository.IRepository;
using System.Linq;
using MultiLayerApp.DataAccess.Domain;

namespace MultiLayerApp.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ShopDbContext _db;

        public ProductRepository(ShopDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product product)
        {
            var objFromDb = _db.Products.FirstOrDefault(s => s.Id == product.Id);
            if (objFromDb != null)
            {
                if (product.Photos != null)
                {
                    objFromDb.Photos = product.Photos;
                }

                objFromDb.Name = product.Name;
                objFromDb.DisplaySize = product.DisplaySize;
                //objFromDb.Manufacturer = product.Manufacturer;
                objFromDb.Memory = product.Memory;
                objFromDb.OperatingSystem = product.OperatingSystem;
                objFromDb.Price = product.Price;
                objFromDb.Processor = product.Processor;
                objFromDb.Size = product.Size;
                objFromDb.Weight = product.Weight;
                objFromDb.CategoryId = product.CategoryId;
            }
        }
    }
}