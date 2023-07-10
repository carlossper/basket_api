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
        //TODO: Add logging and error handling at Controller level
        //Suggestion: Add MediatR to separate concerns
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpPost("create")]
        public IActionResult CreateBasket([FromBody] OrderModel request)
        {
            try
            {
                var basketId = _basketService.CreateBasket();

                var orderLines = request.OrderLines.Select(orderLine => new OrderLineModel
                {
                    ProductId = orderLine.ProductId,
                    ProductName = orderLine.ProductName,
                    ProductUnitPrice = orderLine.ProductUnitPrice,
                    ProductSize = orderLine.ProductSize,
                    Quantity = orderLine.Quantity,
                    TotalPrice = orderLine.TotalPrice
                }).ToList();

                _basketService.AddOrderLinesToBasket(basketId, orderLines);

                return Ok(basketId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{basketId}")]
        public IActionResult GetBasket(Guid basketId)
        {
            try
            {
                var basket = _basketService.GetBasketById(basketId);
                return Ok(basket);
            }
            catch (BasketApiBaseException ex)
            {
                return StatusCode(404, ex.Message);
            }
        }

        [HttpPost("{basketId}/products/{productId}")]
        public async Task<IActionResult> AddProductToBasket(Guid basketId, int productId, int quantity)
        {
            try
            {
                await _basketService.AddProductToBasket(basketId, productId, quantity);
                return Ok(basketId);
            }
            catch (BasketApiBaseException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
