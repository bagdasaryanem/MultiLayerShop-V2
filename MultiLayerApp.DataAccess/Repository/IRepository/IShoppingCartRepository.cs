using MultiLayerApp.DataAccess.Domain;

namespace MultiLayerApp.DataAccess.Repository.IRepository
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        void Update(ShoppingCart obj);
    }
}