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
        Guid CreateBasket();

        /// <summary>
        /// Contract signature for retrieving a BasketModel from the baskets collection from its <paramref name="basketId"/>
        /// </summary>
        /// <param name="basketId">The Basket GUID. </param>
        BasketModel GetBasketById(Guid basketId);

        /// <summary>
        /// Contract signature for Adding a certain <paramref name="quantity"/> of a Product to a Basket.
        /// </summary>
        /// <param name="basketId">The desired Basket ID. </param>
        /// <param name="productId">The desired Product ID to add. </param>
        /// <param name="quantity">The Product quantity to be added. </param>
        Task<BasketModel> AddProductToBasket(Guid basketId, int productId, int quantity);

        /// <summary>
        /// Map a collection of OrderLines to a specific Basket instance. 
        /// </summary>
        /// <param name="basketId">The Basket GUID. </param>
        /// <param name="orderLines">Collection of OrderLines to be added. </param>
        void AddOrderLinesToBasket(Guid basketId, List<OrderLineModel> orderLines);
    }
}
