using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http.Json;
using System.Net;
using BasketApi.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace IntegrationTests
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
        //[DataRow()] Idea would be to add a JSON representation of each product as a resource and pass it for several use cases.
        public async Task GetProductByIdSuccess()
        {
            int productId = 50;
            var expectedProduct = new ProductModel
            {
                Id = productId,
                Name = "White Denim Coat, Size 5",
                Price = 95.77,
                Size = "5",
                Stars = 3
            };

            var response = await _client.GetAsync($"/api/Product/{productId}");

            // Validate HTTP Code
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