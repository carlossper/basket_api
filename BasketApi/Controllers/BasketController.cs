using BasketApi.Exceptions;
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

        //[HttpGet("{basketId}")]

        //[HttpPost("{basketId}/products/{productId}")]

        //[HttpDelete("{basketId}/products/{productId}")]

        //[HttpPut("{basketId}/products/{productId}")]

        //[HttpPost("{basketId}/submit")]
    }
}