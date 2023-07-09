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
        // Implemented like this for ProductService validation only.
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ProductService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
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
                // TODO: Encapsulate authentication even further
                var token = await GetAuthenticationToken();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetAsync("https://azfun-impact-code-challenge-api.azurewebsites.net/api/GetAllProducts");
                var products = Enumerable.Empty<ProductModel>();
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    products = JsonConvert.DeserializeObject<IEnumerable<ProductModel>>(content);
                    return products is null ? Enumerable.Empty<ProductModel>() : products.Take(100);
                }
            }
            catch (HttpRequestException ex)
            {
                throw new BasketApiBaseException("Failed to retrieve the top 100 ranked products from the challenge's API.", ex);
            }

            return Enumerable.Empty<ProductModel>();
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
                // TODO: Encapsulate authentication even further
                var token = await GetAuthenticationToken();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetAsync("https://azfun-impact-code-challenge-api.azurewebsites.net/api/GetAllProducts");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var products = JsonConvert.DeserializeObject<IEnumerable<ProductModel>>(content);

                    var startIndex = (page - 1) * pageSize;
                    var paginatedProducts = products.OrderBy(p => p.Price).Skip(startIndex).Take(pageSize);

                    return paginatedProducts;
                }

                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                throw new BasketApiBaseException("HTTP Request failed to retrieve paginated products from the challenge API.", ex);
            }

            return Enumerable.Empty<ProductModel>();
        }

        /// <summary>
        /// Retreives the top <paramref name="count"/> cheapest product from IMPACT's GetAllProducts endpoint.
        /// </summary>
        /// <param name="count"></param>
        /// <returns>IEnumerable with cheapest products up to <paramref name="count"/>. </returns>
        /// <exception cref="BasketApiBaseException"></exception>
        public async Task<IEnumerable<ProductModel>> GetCheapestProducts(int count)
        {
            try
            {
                // TODO : Move this to a constant and think if it makes sense to reuse this call 
                var response = await _httpClient.GetAsync("https://azfun-impact-code-challenge-api.azurewebsites.net/api/GetAllProducts");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var products = JsonConvert.DeserializeObject<IEnumerable<ProductModel>>(content);
                    return products.OrderBy(p => p.Price).Take(count);
                }

                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                throw new BasketApiBaseException("Failed to retrieve cheapest products from the challenge API.", ex);
            }

            return Enumerable.Empty<ProductModel>();
        }

        #region Private methods
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
