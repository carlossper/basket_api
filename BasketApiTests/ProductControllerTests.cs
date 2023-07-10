using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BasketApi.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace BasketApiIntegrationTests
{
    [TestClass]
    public class ProductControllerTests
    {
        private WebApplicationFactory<Program> _factory;
        private HttpClient _client;

        [TestInitialize]
        public void TestInitialize()
        {
            _factory = new WebApplicationFactory<Program>();
            _client = _factory.CreateClient();
        }

        [TestMethod]
        public async Task GetProductById_ReturnsProduct()
        {
            // Arrange
            int productId = 1;
            var expectedProduct = new ProductModel
            {
                Id = productId,
                Name = "Product 1",
                Price = 10.99,
                Size = "M",
                Stars = 4
            };

            // Act
            var response = await _client.GetAsync($"/api/products/{productId}");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var actualProduct = await response.Content.ReadFromJsonAsync<ProductModel>();
            Assert.IsNotNull(actualProduct);
            Assert.AreEqual(expectedProduct.Id, actualProduct.Id);
            Assert.AreEqual(expectedProduct.Name, actualProduct.Name);
            Assert.AreEqual(expectedProduct.Price, actualProduct.Price);
            Assert.AreEqual(expectedProduct.Size, actualProduct.Size);
            Assert.AreEqual(expectedProduct.Stars, actualProduct.Stars);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _client.Dispose();
            _factory.Dispose();
        }
    }
}