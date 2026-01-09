using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AnnFlowerProject.Services;

namespace AnnFlowerProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// L?y t?t c? s?n ph?m
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _productService.GetAllProductsAsync();
                return Ok(new
                {
                    success = true,
                    message = "L?y danh sách s?n ph?m thành công",
                    data = products,
                    total = products.Count
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Có l?i x?y ra khi l?y danh sách s?n ph?m",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// L?y s?n ph?m theo ID
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);

                if (product == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = $"Không t?m th?y s?n ph?m v?i ID {id}"
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "L?y thông tin s?n ph?m thành công",
                    data = product
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Có l?i x?y ra khi l?y thông tin s?n ph?m",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// L?y s?n ph?m theo danh m?c
        /// </summary>
        [HttpGet("category/{categoryId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductsByCategory(int categoryId)
        {
            try
            {
                var products = await _productService.GetProductsByCategoryAsync(categoryId);

                return Ok(new
                {
                    success = true,
                    message = "L?y danh sách s?n ph?m theo danh m?c thành công",
                    data = products,
                    total = products.Count
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Có l?i x?y ra khi l?y danh sách s?n ph?m",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// L?y t?t c? danh m?c
        /// </summary>
        [HttpGet("categories")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var categories = await _productService.GetAllCategoriesAsync();

                return Ok(new
                {
                    success = true,
                    message = "L?y danh sách danh m?c thành công",
                    data = categories,
                    total = categories.Count
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Có l?i x?y ra khi l?y danh sách danh m?c",
                    error = ex.Message
                });
            }
        }
    }
}
