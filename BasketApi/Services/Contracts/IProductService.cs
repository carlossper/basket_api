using BasketApi.Models;

namespace BasketApi.Services.Contracts
{
    /// <summary>
    /// ProductService contract to fulfill challenge requirements.
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Contract signature for retrieving the first 100 ranked Products from IMPACPT API
        /// </summary>
        Task<IEnumerable<ProductModel>> GetHighestRankedProducts();

        /// <summary>
        /// Contract signature for retrieving paginated Prodcut results from IMPACPT API.
        /// Retrieves <paramref name="page"/> with <paramref name="pageSize"/> number of products.
        /// </summary>
        Task<IEnumerable<ProductModel>> GetPaginatedProductsSortedByPrice(int page, int pageSize);

        /// <summary>
        /// Contract signature for retrieving the first <paramref name="count"/> ranked Products from IMPACPT API.
        /// </summary>
        Task<IEnumerable<ProductModel>> GetCheapestProducts(int count);

        /// <summary>
        /// Contract signature for retrieving a ProductModel instance from its ID.
        /// </summary>
        /// <param name="id">The Id. </param>
        Task<ProductModel> GetProductById(int id);
    }
}