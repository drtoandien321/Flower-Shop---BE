using AnnFlowerProject.Data;
using AnnFlowerProject.Models;
using AnnFlowerProject.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AnnFlowerProject.Repositories.Implementations
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(FlowerShopDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Category>> GetAllWithProductsAsync()
        {
            return await _dbSet
                .Include(c => c.Products)
                .ToListAsync();
        }

        public async Task<Category?> GetByIdWithProductsAsync(int id)
        {
            return await _dbSet
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.CategoryId == id);
        }

        public async Task<Category?> GetByNameAsync(string name)
        {
            return await _dbSet
                .FirstOrDefaultAsync(c => c.CategoryName == name);
        }
    }
}
