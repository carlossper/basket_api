namespace BasketApi.Models.Repositories.Contracts
{
    public interface IBasketRepository
    {
        public Task<IEnumerable<BasketModel>> GetBasketsAsync();
        public Task<BasketModel> GetBasketAsync(int id);
        public  Task CreateBasketAsync(BasketModel basket);
        public Task DeleteBasketAsync(int id);
    }
}
