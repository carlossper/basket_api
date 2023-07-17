using BasketApi.Exceptions;
using BasketApi.Models;
using BasketApi.Models.Repositories.Contracts;
using BasketApi.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace BasketApi.Services.Implementations
{
    public class BasketService : IBasketService
    {
        // Static for persistence accross instances of BasketService
        private static readonly Dictionary<Guid, BasketModel> _baskets = new Dictionary<Guid, BasketModel>();
        private readonly IProductService _productService;
        private readonly IBasketRepository _basketRepository;
        public BasketService(IProductService productService, IBasketRepository basketRepository)
        {
            _productService = productService;
            _basketRepository = basketRepository;
        }

        /// <summary>
        /// Attempts to add <paramref name="quantity"/> amount of a Product for the provided <paramref name="productId"/> to a Basket for the provided <paramref name="basketId"/>.
        /// </summary>
        /// <param name="basketId">The basket GUID. </param>
        /// <param name="productId">The Product ID. </param>
        /// <param name="quantity">The amount of product to be added. </param>
        /// <exception cref="BasketApiBaseException"></exception>
        public async Task<BasketModel> AddProductToBasket(Guid basketId, int productId, int quantity)
        {
            BasketModel basket = GetBasketById(basketId);

            var product = await _productService.GetProductById(productId);
            if (product == null)
            {
                throw new BasketApiBaseException($"Product with ID {productId} not found.");
            }

            OrderLineModel? existingOrderLine = basket.OrderLines.FirstOrDefault(ol => ol.ProductId == productId);
            if (existingOrderLine != null)
            {
                existingOrderLine.Quantity += quantity;
                existingOrderLine.TotalPrice = (existingOrderLine.Quantity * existingOrderLine.ProductUnitPrice);
            }
            else
            {
                var newOrderLine = new OrderLineModel
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ProductUnitPrice = product.Price,
                    ProductSize = product.Size,
                    Quantity = quantity,
                    TotalPrice = product.Price * quantity
                };
                basket.OrderLines.Add(newOrderLine);
            }

            return basket;
        }

        /// <summary>
        /// Creates a new BasketModel instance with a unique GUID
        /// </summary>
        /// <returns>The GUID of the created BasketModel instance. </returns>
        public Guid CreateBasket()
        {
            var basket = new BasketModel
            {
                Id = Guid.NewGuid()
            };

            // Map the new BasketId to the BasketModel instance. 
            _baskets[basket.Id] = basket;

            return basket.Id;
        }

        /// <summary>
        /// Adds a collection of OrderLines to a BasketModel.
        /// </summary>
        /// <param name="basketId">The basket GUID. </param>
        /// <param name="orderLines">The collection of OrderLines. </param>
        public void AddOrderLinesToBasket(Guid basketId, List<OrderLineModel> orderLines)
        {
            var basket = GetBasketById(basketId);
            basket.OrderLines.AddRange(orderLines);
        }

        /// <summary>
        /// Attempts to retrieve the BasketModel instance from the _baskets collection from a provided ID.
        /// </summary>
        /// <param name="basketId">The Basket GUID. </param>
        /// <returns></returns>
        /// <exception cref="BasketApiBaseException"></exception>
        public BasketModel GetBasketById(Guid basketId)
        {
            if (_baskets.TryGetValue(basketId, out var basket))
            {
                return basket;
            }
            else
            {
                throw new BasketApiBaseException($"BasketModel with ID {basketId} not found.");
            }
        }

        public async Task<IActionResult> GetBasketById(int id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);

            if (basket == null)
            {
                return NotFound();
            }

            return Ok(basket);
        }
    }
}
