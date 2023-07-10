using BasketApi.Exceptions;
using BasketApi.Models;
using BasketApi.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace BasketApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        //TODO: Add logging and error handling at Controller level
        //Suggestion: Add MediatR to separate concerns
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var product = await _productService.GetProductById(id);
                if (product == null)
                {
                    return NotFound();
                }

                return Ok(product);
            }
            catch (BasketApiBaseException ex)
            {
                // Log the exception message once logging is added
                return StatusCode(500, "Could");
            }
        }

        [HttpGet("highest-ranked")]
        public async Task<IEnumerable<ProductModel>> GetHighestRankedProducts()
        {
            return await _productService.GetHighestRankedProducts();    
        }

        [HttpGet("paginated")]
        public async Task<IEnumerable<ProductModel>> GetPaginatedProducts(int pageNumber, int pageSize)
        {
            return await _productService.GetPaginatedProductsSortedByPrice(pageNumber, pageSize);
        }

        [HttpGet("cheapest")]
        public async Task<IEnumerable<ProductModel>> GetTop10CheapestProducts()
        {
            return await _productService.GetCheapestProducts(10);
        }
    }
}
