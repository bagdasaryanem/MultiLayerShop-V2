using MultiLayerApp.DataAccess.Domain;

namespace MultiLayerApp.DataAccess.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product product);
    }
}