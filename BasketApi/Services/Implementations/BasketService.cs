using BasketApi.Exceptions;
using BasketApi.Models;
using BasketApi.Services.Contracts;
using System.Collections.Concurrent;

namespace BasketApi.Services.Implementations
{
    public class BasketService : IBasketService
    {
        private readonly ConcurrentDictionary<Guid, BasketModel> _baskets;
        private readonly IProductService _productService;

        public BasketService(IProductService productService)
        {
            _baskets = new ConcurrentDictionary<Guid, BasketModel>();
            _productService = productService;
        }

        public Task AddProductToBasket(Guid basketId, int productId, int quantity)
        {
            throw new NotImplementedException();
        }

        public async Task<Guid> CreateBasket()
        {
            var basket = new BasketModel
            {
                Id = Guid.NewGuid(),
                OrderLines = new List<OrderLineModel>()
            };

            // Map the new BasketId to the BasketModel instance. 
            _baskets[basket.Id] = basket;

            return basket.Id;
        }

        public async Task<BasketModel> GetBasketById(Guid basketId)
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

        public Task<IEnumerable<ProductModel>> GetCheapestProducts()
        {
            throw new NotImplementedException();
        }

    }
}
