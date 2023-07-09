using BasketApi.Models;
using BasketApi.Services.Contracts;

namespace BasketApi.Services.Implementations
{
    public class BasketService : IBasketService
    {
        public async Task<BasketModel> AddProduct(ProductModel product)
        {
            throw new NotImplementedException();
        }

        public async Task<BasketModel> RemoveProduct(int productId)
        {
            throw new NotImplementedException();
        }
    }
}
