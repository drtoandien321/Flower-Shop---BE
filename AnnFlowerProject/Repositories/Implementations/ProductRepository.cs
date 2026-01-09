using AnnFlowerProject.Data;
using AnnFlowerProject.Models;
using AnnFlowerProject.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AnnFlowerProject.Repositories.Implementations
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(FlowerShopDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Product>> GetAllWithCategoryAsync()
        {
            return await _dbSet
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<Product?> GetByIdWithCategoryAsync(int id)
        {
            return await _dbSet
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId)
        {
            return await _dbSet
                .Include(p => p.Category)
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> SearchByNameAsync(string keyword)
        {
            return await _dbSet
                .Include(p => p.Category)
                .Where(p => p.ProductName.Contains(keyword) || 
                           p.Description.Contains(keyword))
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsInStockAsync()
        {
            return await _dbSet
                .Include(p => p.Category)
                .Where(p => p.StockQuantity > 0)
                .ToListAsync();
        }
    }
}
