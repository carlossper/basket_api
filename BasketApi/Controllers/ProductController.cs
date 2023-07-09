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
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
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
