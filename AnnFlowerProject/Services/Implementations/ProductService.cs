using AnnFlowerProject.DTOs;
using AnnFlowerProject.Models;
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

        #region Read Operations

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

        #endregion

        #region Create, Update, Delete Operations

        public async Task<ProductDto?> CreateProductAsync(CreateProductDto createDto)
        {
            // Validate category exists
            var category = await _unitOfWork.Categories.GetByIdAsync(createDto.CategoryId);
            if (category == null)
            {
                return null; // Category không t?n t?i
            }

            // Create new product
            var product = new Product
            {
                ProductName = createDto.ProductName,
                UnitPrice = createDto.UnitPrice,
                StockQuantity = createDto.StockQuantity,
                ImageURL = createDto.ImageURL,
                Description = createDto.Description,
                CategoryId = createDto.CategoryId
            };

            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            // Load category information
            var createdProduct = await _unitOfWork.Products.GetByIdWithCategoryAsync(product.ProductId);

            if (createdProduct == null)
                return null;

            return new ProductDto
            {
                ProductId = createdProduct.ProductId,
                ProductName = createdProduct.ProductName,
                UnitPrice = createdProduct.UnitPrice,
                StockQuantity = createdProduct.StockQuantity,
                ImageURL = createdProduct.ImageURL,
                Description = createdProduct.Description,
                CategoryId = createdProduct.CategoryId,
                CategoryName = createdProduct.Category?.CategoryName ?? "N/A"
            };
        }

        public async Task<ProductDto?> UpdateProductAsync(int id, UpdateProductDto updateDto)
        {
            // Get existing product
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
            {
                return null; // Product không t?n t?i
            }

            // Validate category exists
            var category = await _unitOfWork.Categories.GetByIdAsync(updateDto.CategoryId);
            if (category == null)
            {
                return null; // Category không t?n t?i
            }

            // Update product properties
            product.ProductName = updateDto.ProductName;
            product.UnitPrice = updateDto.UnitPrice;
            product.StockQuantity = updateDto.StockQuantity;
            product.ImageURL = updateDto.ImageURL;
            product.Description = updateDto.Description;
            product.CategoryId = updateDto.CategoryId;

            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveChangesAsync();

            // Return updated product with category
            var updatedProduct = await _unitOfWork.Products.GetByIdWithCategoryAsync(id);

            if (updatedProduct == null)
                return null;

            return new ProductDto
            {
                ProductId = updatedProduct.ProductId,
                ProductName = updatedProduct.ProductName,
                UnitPrice = updatedProduct.UnitPrice,
                StockQuantity = updatedProduct.StockQuantity,
                ImageURL = updatedProduct.ImageURL,
                Description = updatedProduct.Description,
                CategoryId = updatedProduct.CategoryId,
                CategoryName = updatedProduct.Category?.CategoryName ?? "N/A"
            };
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            
            if (product == null)
            {
                return false; // Product không t?n t?i
            }

            _unitOfWork.Products.Remove(product);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        #endregion
    }
}
