

using Core.Interface;
using Core.Models;
using Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Только админы могут использовать этот контроллер
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductsController(IProductService productService,ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
          
            try
            {
                var products = await _productService.GetProductsAsync();
                var categories = await _categoryService.GetCategoriesAsync();
                var categoryDict = categories.ToDictionary(c => c.CategoryId, c => c);

                foreach (var product in products)
                {
                    if (categoryDict.TryGetValue(product.CategoryId, out var category))
                    {
                        product.Category = category;
                    }
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting products: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [Produces("application/json")]
        public async Task<ActionResult<Product>> GetProduct(Guid id)
        {
        
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    return NotFound();
                }
                product.Category = await _categoryService.GetCategoryByIdAsync(product.CategoryId);
                return Ok(product);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting product by ID {id}: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("add")]
        [AllowAnonymous]
        public async Task<ActionResult<Product>> AddProduct([FromBody] Product product)
        {
          
            Console.WriteLine("AddProduct endpoint hit...");
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Invalid model state");
                return BadRequest(ModelState);
            }

            try
            {
                Console.WriteLine("Model state is valid, adding product...");
                var createdProduct = await _productService.AddProductAsync(product);
                Console.WriteLine("Product added, returning CreatedAtAction result");
                return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.ProductId }, createdProduct);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding product: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] Product product)
        {
          
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _productService.UpdateProductAsync(product);
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating product: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            
            try
            {
                var result = await _productService.DeleteProductAsync(id);
                if (!result)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting product: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
