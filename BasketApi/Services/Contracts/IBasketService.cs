using BasketApi.Models;

namespace BasketApi.Services.Contracts
{
    /// <summary>
    /// BasketService contract to fulfill challenge requirements.
    /// </summary>
    public interface IBasketService
    {
        /// <summary>
        /// Contract signature for Creating a new BasketModel entity.
        /// </summary>
        Task<Guid> CreateBasket();

        /// <summary>
        /// Contract signature for retrieving a BasketModel from the baskets collection from its <paramref name="basketId"/>
        /// </summary>
        /// <param name="basketId"> </param>
        Task<BasketModel> GetBasketById(Guid basketId);

        /// <summary>
        /// Contract signature for Adding a certain <paramref name="quantity"/> of a Product to a Basket.
        /// </summary>
        /// <param name="basketId">The desired Basket ID. </param>
        /// <param name="productId">The desired Product ID to add. </param>
        /// <param name="quantity">The Product quantity to be added. </param>
        Task AddProductToBasket(Guid basketId, int productId, int quantity);
    }
}
