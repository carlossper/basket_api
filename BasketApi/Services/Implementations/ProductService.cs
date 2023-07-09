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
        private readonly List<ProductModel>? _products;

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
                if (_products.Count == 0)
                {
                    await GetAllProductsByRankDescending();
                }

                return _products.Take(100);
            }
            catch (HttpRequestException ex)
            {
                throw new BasketApiBaseException("Failed to retrieve the top 100 ranked products from the challenge's API.", ex);
            }
        }

        /// <summary>
        /// Gets a ProductModel instance for the provided <paramref name="productId"/>
        /// </summary>
        /// <param name="productId">The Product ID to search for. </param>
        /// <returns>The ProductModel instance for productId</returns>
        public async Task<ProductModel> GetProductById(int productId)
        {
            if (_products.Count == 0)
            {
                await GetAllProductsByRankDescending();
            }

            return _products.FirstOrDefault(p => p.Id == productId);
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
                if (_products.Count == 0)
                {
                    await GetAllProductsByPriceAscending();
                }

                var startIndex = (page - 1) * pageSize;
                return _products.Skip(startIndex).Take(pageSize);
            }
            catch (HttpRequestException ex)
            {
                throw new BasketApiBaseException("HTTP Request failed to retrieve paginated products from the challenge API.", ex);
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
                if (_products.Count == 0)
                {
                    await GetAllProductsByPriceAscending();
                }

                return _products.Take(count);
            }
            catch (HttpRequestException ex)
            {
                throw new BasketApiBaseException("Failed to retrieve cheapest products from the challenge API.", ex);
            }

            return Enumerable.Empty<ProductModel>();
        }

        #region Private methods
        /// <summary>
        /// Gets all 10000 Products from the challenge's API and stores them in the _products lists ordered ascending by Stars/Rank.
        /// </summary>
        /// <returns></returns>
        private async Task GetAllProductsByRankDescending()
        {
            try
            {
                // TODO: Encapsulate authentication even further
                var token = await GetAuthenticationToken();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetAsync("https://azfun-impact-code-challenge-api.azurewebsites.net/api/GetAllProducts");
                var products = Enumerable.Empty<ProductModel>();
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    _products.AddRange(JsonConvert.DeserializeObject<IEnumerable<ProductModel>>(content).OrderByDescending(p => p.Stars));
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new BasketApiBaseException("Failed to retrieve products from the challenge API.");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new BasketApiBaseException("Failed to retrieve and set all Products from the challenge's API.", ex);
            }
        }

        /// <summary>
        /// Gets all 10000 Products from the challenge's API and stores them in the _products lists odered by Price.
        /// </summary>
        private async Task GetAllProductsByPriceAscending()
        {
            try
            {
                // TODO: Encapsulate authentication even further
                var token = await GetAuthenticationToken();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetAsync("https://azfun-impact-code-challenge-api.azurewebsites.net/api/GetAllProducts");
                var products = Enumerable.Empty<ProductModel>();
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var prodsTemp = JsonConvert.DeserializeObject<IEnumerable<ProductModel>>(content).OrderBy(p => p.Price);    
                    _products.AddRange(prodsTemp);
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new BasketApiBaseException("Failed to retrieve products from the challenge API.");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new BasketApiBaseException("Failed to retrieve and set all Products from the challenge's API.", ex);
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
                var loginEndpointUrl = "https://azfun-impact-code-challenge-api.azurewebsites.net/api/Login";
                var email = "c_m_per@hotmail.com";

                // Create a JSON payload with my email
                var payload = new { Email = email };
                var jsonPayload = JsonConvert.SerializeObject(payload);
                var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                // Send the POST request to the Login endpoint
                var response = await _httpClient.PostAsync(loginEndpointUrl, httpContent);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    var token = JObject.Parse(responseContent)["token"].Value<string>();

                    return token;
                }
                else throw new BasketApiBaseException("HTTP Request failed to retrieve the authentication token.");
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
