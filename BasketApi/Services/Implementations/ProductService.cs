using BasketApi.Exceptions;
using BasketApi.Models;
using BasketApi.Services.Contracts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace BasketApi.Services.Implementations
{
    /// <summary>
    /// Entity responsible for encapsulating any Product related operations to be performed. 
    /// </summary>
    public class ProductService : IProductService
    {
        private List<ProductModel>? _products;

        // TODO: Remove hard dependency on HttpClient here.
        // TODO: Remove all meaningful warnings
        // TODO: Refactor calls to use GetAllProductsByRankDescending method : DONE
        // Implemented like this for ProductService validation only.
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ProductService(HttpClient httpClient, IConfiguration configuration)

        {
            _httpClient = httpClient;
            _configuration = configuration;
            _products = new List<ProductModel>();
        }

        /// <summary>
        /// Retrieves the 100 highest ranked products
        /// </summary>
        /// <returns></returns>
        /// <exception cref="BasketApiBaseException"></exception>
        public async Task<IEnumerable<ProductModel>> GetHighestRankedProducts()
        {
            try
            {
                if (_products is null || _products.Count == 0)
                {
                    await GetAllProducts();
                }

                return _products.OrderByDescending(p => p.Stars).Take(100);
            }
            catch (HttpRequestException ex)
            {
                throw new BasketApiBaseException("Failed to retrieve the top 100 ranked products.", ex);
            }
        }

        /// <summary>
        /// Gets a ProductModel instance for the provided <paramref name="productId"/>
        /// </summary>
        /// <param name="productId">The Product ID to search for. </param>
        /// <returns>The ProductModel instance for productId</returns>
        public async Task<ProductModel> GetProductById(int productId)
        {
            if (_products is null || _products.Count == 0)
            {
                await GetAllProducts();
            }

            ProductModel product = _products.FirstOrDefault(p => p.Id == productId);

            if (product is null)
            {
                throw new NotFoundException($"Could not find Product with ID: {productId}.");
            }

            return product;
        }

        /// <summary>
        /// Retrieves Paginated products according with Page number as <paramref name="page"/> and Page size as <paramref name="pageSize"/>
        /// </summary>
        /// <param name="page">The page to retrieve the products to. </param>
        /// <param name="pageSize">The number of products per page. </param>
        /// <returns></returns>
        public async Task<IEnumerable<ProductModel>> GetPaginatedProductsSortedByPrice(int page, int pageSize)
        {
            pageSize = GetPageSize(pageSize);
            try
            {
                if (_products is null || _products.Count == 0)
                {
                    await GetAllProducts();
                }

                int startIndex = (page - 1) * pageSize;
                return _products.OrderBy(p => p.Price).Skip(startIndex).Take(pageSize);
            }
            catch (HttpRequestException ex)
            {
                // TODO: Log exception details
                return Enumerable.Empty<ProductModel>();
            }
        }

        /// <summary>
        /// Retreives the top <paramref name="count"/> cheapest product from IMPACT's GetAllProductsByRankDescending endpoint.
        /// </summary>
        /// <param name="count"></param>
        /// <returns>IEnumerable with cheapest products up to <paramref name="count"/>. </returns>
        /// <exception cref="BasketApiBaseException"></exception>
        public async Task<IEnumerable<ProductModel>> GetCheapestProducts(int count)
        {
            try
            {
                if (_products is null || _products.Count == 0)
                {
                    await GetAllProducts();
                }

                return _products.OrderBy(p => p.Price).Take(count);
            }
            catch (HttpRequestException ex)
            {
                // TODO: Log exception details
                return Enumerable.Empty<ProductModel>();
            }
        }

        #region Private methods
        /// <summary>
        /// Gets all Products from /GetAllProducts endpoint. 
        /// </summary>
        /// <exception cref="BasketApiBaseException"></exception>
        private async Task GetAllProducts()
        {
            try
            {
                string token = await GetAuthenticationToken();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync("https://azfun-impact-code-challenge-api.azurewebsites.net/api/GetAllProducts");

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    _products = JsonConvert.DeserializeObject<List<ProductModel>>(content);
                }
                else
                {
                    throw new BasketApiBaseException("Failed to retrieve products from the challenge API.");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new BasketApiBaseException("Failed to retrieve and set all products from the challenge's API.", ex);
            }
        }

        /// <summary>
        /// Gets authentication Bearer token for my email.
        /// </summary>
        /// <returns>Bearer Token.</returns>
        private async Task<string> GetAuthenticationToken()
        {
            try
            {
                string loginEndpointUrl = "https://azfun-impact-code-challenge-api.azurewebsites.net/api/Login";
                string email = "c_m_per@hotmail.com";

                var payload = new { Email = email };
                string jsonPayload = JsonConvert.SerializeObject(payload);
                var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(loginEndpointUrl, httpContent);

                if (!response.IsSuccessStatusCode)
                {
                    throw new BasketApiBaseException("HTTP Request failed to retrieve the authentication token.");
                }

                string responseContent = await response.Content.ReadAsStringAsync();

                return JObject.Parse(responseContent)["token"].Value<string>();
            }
            catch (HttpRequestException ex)
            {
                throw new BasketApiBaseException("HTTP Request failed to retrieve the authentication token.", ex);
            }
            catch (Exception ex)
            {
                throw new BasketApiBaseException("Generic error during GetAuthenticationToken()", ex);
            }
        }

        /// <summary>
        /// Gets the page size for pagination purposes according to 1000 max constraint.
        /// </summary>
        /// <param name="pageSize">The requested page size. </param>
        /// <returns>The page size after constraint validation. </returns>
        private static int GetPageSize(int pageSize)
        {
            pageSize = Math.Min(pageSize, 1000);
            return pageSize;
        }
        #endregion
    }
}