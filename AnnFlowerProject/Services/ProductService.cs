using AnnFlowerProject.DTOs;
using AnnFlowerProject.UnitOfWork;

namespace AnnFlowerProject.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ProductListDto>> GetAllProductsAsync()
        {
            var products = await _unitOfWork.Products.GetAllWithCategoryAsync();
            
            return products.Select(p => new ProductListDto
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                UnitPrice = p.UnitPrice,
                StockQuantity = p.StockQuantity,
                ImageURL = p.ImageURL,
                CategoryName = p.Category?.CategoryName ?? "N/A"
            }).ToList();
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdWithCategoryAsync(id);

            if (product == null)
                return null;

            return new ProductDto
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                UnitPrice = product.UnitPrice,
                StockQuantity = product.StockQuantity,
                ImageURL = product.ImageURL,
                Description = product.Description,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.CategoryName ?? "N/A"
            };
        }

        public async Task<List<ProductListDto>> GetProductsByCategoryAsync(int categoryId)
        {
            var products = await _unitOfWork.Products.GetByCategoryIdAsync(categoryId);
            
            return products.Select(p => new ProductListDto
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                UnitPrice = p.UnitPrice,
                StockQuantity = p.StockQuantity,
                ImageURL = p.ImageURL,
                CategoryName = p.Category?.CategoryName ?? "N/A"
            }).ToList();
        }

        public async Task<List<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllWithProductsAsync();
            
            return categories.Select(c => new CategoryDto
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                ProductCount = c.Products.Count
            }).ToList();
        }
    }
}
