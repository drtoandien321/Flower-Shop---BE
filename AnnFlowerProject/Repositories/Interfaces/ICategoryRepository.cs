using AnnFlowerProject.Models;

namespace AnnFlowerProject.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IEnumerable<Category>> GetAllWithProductsAsync();
        Task<Category?> GetByIdWithProductsAsync(int id);
        Task<Category?> GetByNameAsync(string name);
    }
}
