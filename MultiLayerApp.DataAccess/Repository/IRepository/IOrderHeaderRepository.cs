
using MultiLayerApp.DataAccess.Domain;

namespace MultiLayerApp.DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void Update(OrderHeader obj);
    }
}