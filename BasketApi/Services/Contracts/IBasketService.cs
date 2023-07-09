using BasketApi.Models;

namespace BasketApi.Services.Contracts
{
    /// <summary>
    /// BasketService contract to fulfill challenge requirements.
    /// </summary>
    public interface IBasketService
    {
        /// <summary>
        /// Contract signature for adding <paramref name="product"/> to a user's basket.
        /// </summary>
        /// <param name="product">The Product DTO to be added to the basket. </param>
        /// <returns>The updated Basket DTO. </returns>
        Task<BasketModel> AddProduct(ProductModel product);

        /// <summary>
        /// Contract signature for removing the Product associated with <paramref name="productId"/> 
        /// from a user's basket.
        /// </summary>
        /// <param name="product">The ProductID for the product to be removed. </param>
        /// <returns>The updated Basket DTO. </returns>
        Task<BasketModel> RemoveProduct(int productId);
    }
}
