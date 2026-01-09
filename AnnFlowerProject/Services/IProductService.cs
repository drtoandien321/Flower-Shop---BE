using AnnFlowerProject.DTOs;

namespace AnnFlowerProject.Services
{
    public interface IProductService
    {
        Task<List<ProductListDto>> GetAllProductsAsync();
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<List<ProductListDto>> GetProductsByCategoryAsync(int categoryId);
        Task<List<CategoryDto>> GetAllCategoriesAsync();
    }
}
