using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http.Json;
using System.Net;
using BasketApi.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System.ComponentModel;
using DescriptionAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute;

namespace IntegrationTests
{
    [TestClass]
    public class ProductControllerTests : ControllerTestsBase
    {
        [TestMethod]
        [DataRow(50, "White Denim Coat, Size 5", 95.77, "5", 3)]
        [DataRow(100, "Yellow Denim Shirt, Size 37", 81.98, "37", 4)] 
        [DataRow(200, "White Velvet Jacket, Size 51", 97.26, "51", 5)]
        public async Task GetProductByIdSuccess(int testProductId, string expectedName, double expectedPrice, string expectedSize, int expectedRank)
        {
            var expectedProduct = new ProductModel
            {
                Id = testProductId,
                Name = expectedName,
                Price = expectedPrice,
                Size = expectedSize,
                Stars = expectedRank
            };

            var response = await Client.GetAsync($"/api/Product/{testProductId}");

            // Validate HTTP Code
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var actualProduct = await response.Content.ReadFromJsonAsync<ProductModel>();
            Assert.IsNotNull(actualProduct);
            Assert.AreEqual(expectedProduct.Id, actualProduct.Id);
            Assert.AreEqual(expectedProduct.Name, actualProduct.Name, actualProduct.ToString());
            Assert.AreEqual(expectedProduct.Price, actualProduct.Price);
            Assert.AreEqual(expectedProduct.Size, actualProduct.Size);
            Assert.AreEqual(expectedProduct.Stars, actualProduct.Stars);
        }

        [TestMethod]
        [DataRow(10005, HttpStatusCode.InternalServerError)]
        [Description("Product ID over 10.000 => Internal Server Error is returned")]
        public async Task GetProductByIdStatusErrors(int testProductId, HttpStatusCode expectedStatusCode)
        {
            var response = await Client.GetAsync($"/api/Product/{testProductId}");

            // Validate HTTP Code
            Assert.AreEqual(expectedStatusCode, response.StatusCode);
        }
    }
}