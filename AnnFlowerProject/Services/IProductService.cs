using AnnFlowerProject.DTOs;

namespace AnnFlowerProject.Services
{
    public interface IProductService
    {
        // Read operations
        Task<List<ProductListDto>> GetAllProductsAsync();
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<List<ProductListDto>> GetProductsByCategoryAsync(int categoryId);
        Task<List<CategoryDto>> GetAllCategoriesAsync();
        
        // Create, Update, Delete operations
        Task<ProductDto?> CreateProductAsync(CreateProductDto createDto);
        Task<ProductDto?> UpdateProductAsync(int id, UpdateProductDto updateDto);
        Task<bool> DeleteProductAsync(int id);
    }
}
