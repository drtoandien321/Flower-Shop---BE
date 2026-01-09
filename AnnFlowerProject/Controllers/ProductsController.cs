using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AnnFlowerProject.DTOs;
using AnnFlowerProject.Services.Interfaces;

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

        #region Read Operations (Public)

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

        #endregion

        #region Create, Update, Delete Operations (Admin Only)

        /// <summary>
        /// T?o s?n ph?m m?i (Admin only)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "D? li?u kh?ng h?p l?",
                        errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                    });
                }

                var product = await _productService.CreateProductAsync(createDto);

                if (product == null)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Kh?ng th? th?c hi?n t?o s?n ph?m. Vui l?ng ki?m tra CategoryId c? t?n t?ng h?p l? kh?ng."
                    });
                }

                return CreatedAtAction(
                    nameof(GetProductById),
                    new { id = product.ProductId },
                    new
                    {
                        success = true,
                        message = "T?o s?n ph?m th?nh c?ng",
                        data = product
                    });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Có l?i x?y ra khi t?o s?n ph?m",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// C?p nh?t s?n ph?m (Admin only)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "D? li?u kh?ng h?p l?",
                        errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                    });
                }

                var product = await _productService.UpdateProductAsync(id, updateDto);

                if (product == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = $"Kh?ng t?m th?y s?n ph?m v?i ID {id} ho?c CategoryId kh?ng h?p l?"
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "C?p nh?t s?n ph?m th?nh c?ng",
                    data = product
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Có l?i x?y ra khi c?p nh?t s?n ph?m",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// X?a s?n ph?m (Admin only)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var result = await _productService.DeleteProductAsync(id);

                if (!result)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = $"Kh?ng t?m th?y s?n ph?m v?i ID {id}"
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "X?a s?n ph?m th?nh c?ng"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Có l?i x?y ra khi xóa s?n ph?m",
                    error = ex.Message
                });
            }
        }

        #endregion
    }
}
