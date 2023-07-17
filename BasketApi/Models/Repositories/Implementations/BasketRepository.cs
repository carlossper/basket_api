using BasketApi.Models.Contexts.Implementations;
using BasketApi.Models.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BasketApi.Models.Repositories.Implementations
{
    public class BasketRepository : IBasketRepository
    {
        private readonly BasketContext _basketContext;

        public BasketRepository(BasketContext basketContext)
        {
            _basketContext = basketContext;
        }

        public async Task<IEnumerable<BasketModel>> GetBasketsAsync()
        {
            return await _basketContext.Baskets.ToListAsync();
        }

        public async Task<BasketModel> GetBasketAsync(int id)
        {
            var basket = await _basketContext.Baskets.FindAsync(id);

            if (basket == null)
            {
                return null;
            }

            return basket;
        }

        public async Task CreateBasketAsync(BasketModel basket)
        {
            await _basketContext.Baskets.AddAsync(basket);
            await _basketContext.SaveChangesAsync();
        }

        public async Task DeleteBasketAsync(int id)
        {
            var basket = await _basketContext.Baskets.FindAsync(id);

            if (basket == null)
            {
                return;
            }

            _basketContext.Baskets.Remove(basket);
            await _basketContext.SaveChangesAsync();
        }
    }
}
