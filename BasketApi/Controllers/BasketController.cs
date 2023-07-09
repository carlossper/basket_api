using BasketApi.Models;
using BasketApi.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BasketApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpPost]
        public async Task<BasketModel> AddProductToBasket([FromBody] ProductModel product)
        {
            var basket = await _basketService.AddProduct(product);
            return basket;
        }

        [HttpDelete("{productId}")]
        public async Task<BasketModel> RemoveProductFromBasket(int productId)
        {
            var basket = await _basketService.RemoveProduct(productId);
            return basket;
        } 
    }
}