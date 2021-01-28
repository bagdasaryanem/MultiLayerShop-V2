using MultiLayerApp.DataAccess.Domain;

namespace MultiLayerApp.DataAccess.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void Update(Category category);
    }
}