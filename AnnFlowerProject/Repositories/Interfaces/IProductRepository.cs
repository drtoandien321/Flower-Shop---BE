using AnnFlowerProject.Models;

namespace AnnFlowerProject.Repositories.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetAllWithCategoryAsync();
        Task<Product?> GetByIdWithCategoryAsync(int id);
        Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId);
        Task<IEnumerable<Product>> SearchByNameAsync(string keyword);
        Task<IEnumerable<Product>> GetProductsInStockAsync();
    }
}
