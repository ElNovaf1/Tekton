using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using Tekton.Service.DTO;

namespace Tekton.IntegrationTest
{
    [TestClass]
    public class ProductControllerTest
    {
        IConfiguration _config;
        private HttpClient _httpClient;
        private string BaseUri;

        public ProductControllerTest()
        {
            _config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            BaseUri = _config["IntegrationTestBaseUri"]!;
            var WebAppFactory = new WebApplicationFactory<Program>();
            _httpClient = WebAppFactory.CreateDefaultClient();
        }

        [TestMethod]
        public async Task GetById_WhenCalledAndProductExists_ReturnsProductDTO()
        {
            long cId = 1;
            string Uri = BaseUri + "api/Products/" + cId.ToString();
            var response = await _httpClient.GetAsync(Uri);
            var result = await response.Content.ReadAsStringAsync();
            var cInfo = JsonConvert.DeserializeObject<ProductDTO>(result);

            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsInstanceOfType(cInfo, typeof(ProductDTO));
            Assert.IsTrue(cId == cInfo.ProductId);
        }
        [TestMethod]
        public async Task GetById_WhenCalledAndProductNotExists_ReturnsNotFound()
        {
            long cId = 0;
            string Uri = BaseUri + "api/Products/" + cId.ToString();
            var response = await _httpClient.GetAsync(Uri);
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.NotFound);
        }
        [TestMethod]
        public async Task GetAll_WhenCalledAndProductsExists_ReturnsListOfProductDTO()
        {
            string Uri = BaseUri + "api/Products/";
            var response = await _httpClient.GetAsync(Uri);
            var result = await response.Content.ReadAsStringAsync();
            var cInfo = JsonConvert.DeserializeObject<List<ProductDTO>>(result);

            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsInstanceOfType(cInfo, typeof(List<ProductDTO>));
        }

        [TestMethod]
        public async Task Insert_WhenCalledAndSuccess_ReturnsInsertedProductDTO()
        {
            var rand = new Random();
            var randomName = string.Join("", Enumerable.Repeat(0, 10).Select(n => (char)rand.Next(127)));

            NewProductDTO cInfo = new NewProductDTO()
            {
                Name = randomName,
                Description = new Random().Next(1000).ToString(),
                StatusId = 1,
                Price = new Random().Next(1000),
                Stock = 1
            };

            string Uri = BaseUri + "api/Products/";
            var response = await _httpClient.PostAsync(Uri, new StringContent(JsonConvert.SerializeObject(cInfo), Encoding.UTF8, "application/json"));
            var result = await response.Content.ReadAsStringAsync();
            var cOutputInfo = JsonConvert.DeserializeObject<ProductDTO>(result);

            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsTrue(cOutputInfo.ProductId > 0);
        }
        [TestMethod]
        public async Task Insert_WhenCalledAndStatusIsNotValid_ReturnsBadRequest()
        {
            var rand = new Random();
            var randomName = string.Join("", Enumerable.Repeat(0, 10).Select(n => (char)rand.Next(127)));

            NewProductDTO cInfo = new NewProductDTO()
            {
                Name = randomName,
                Description = new Random().Next(1000).ToString(),
                StatusId = 200,
                Price = new Random().Next(1000),
                Stock = 1
            };

            string Uri = BaseUri + "api/Products/";
            var response = await _httpClient.PostAsync(Uri, new StringContent(JsonConvert.SerializeObject(cInfo), Encoding.UTF8, "application/json"));
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task Insert_WhenCalledAndProductAlreadyExists_ReturnsBadRequest()
        {
            string Uri = BaseUri + "api/Products/";
            var response = await _httpClient.GetAsync(Uri);
            var result = await response.Content.ReadAsStringAsync();
            var cInfo = JsonConvert.DeserializeObject<List<ProductDTO>>(result);

            NewProductDTO cInfoInsert = new NewProductDTO()
            {
                Name = cInfo[0].Name,
                Description = cInfo[0].Description,
                StatusId = 1,
                Price = 55,
                Stock = 1
            };

            var responseInsert = await _httpClient.PostAsync(Uri, new StringContent(JsonConvert.SerializeObject(cInfoInsert), Encoding.UTF8, "application/json"));
            Assert.IsTrue(responseInsert.StatusCode == System.Net.HttpStatusCode.BadRequest);
        }
        [TestMethod]
        public async Task Update_WhenCalledAndProductNoExists_ReturnsNotFound()
        {
            var rand = new Random();
            var randomName = string.Join("", Enumerable.Repeat(0, 10).Select(n => (char)rand.Next(127)));

            EditProductDTO cInfo = new EditProductDTO()
            {
                ProductId = 0,
                Name = randomName,
                Description = new Random().Next(1000).ToString(),
                StatusId = 1,
                Price = new Random().Next(1000),
                Stock = 1
            };

            string Uri = BaseUri + "api/Products/";
            var response = await _httpClient.PutAsync(Uri, new StringContent(JsonConvert.SerializeObject(cInfo), Encoding.UTF8, "application/json"));
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.NotFound);
        }
        [TestMethod]
        public async Task Update_WhenCalledAndDataIsNotValid_ReturnsBadRequest()
        {
            var rand = new Random();
            var randomName = string.Join("", Enumerable.Repeat(0, 10).Select(n => (char)rand.Next(127)));

            NewProductDTO cInfo = new NewProductDTO()
            {
                Name = randomName,
                Description = new Random().Next(1000).ToString(),
                StatusId = 1,
                Price = new Random().Next(1000),
                Stock = 1
            };

            string Uri = BaseUri + "api/Products/";
            var response = await _httpClient.PostAsync(Uri, new StringContent(JsonConvert.SerializeObject(cInfo), Encoding.UTF8, "application/json"));
            var result = await response.Content.ReadAsStringAsync();
            var cOutputInfo = JsonConvert.DeserializeObject<ProductDTO>(result);

            EditProductDTO cInfoInserted = new EditProductDTO()
            {
                ProductId = cOutputInfo.ProductId,
                Name = cOutputInfo.Name,
                Description = cOutputInfo.Description,
                StatusId = 200,
                Price = cOutputInfo.Price,
                Stock = cOutputInfo.Stock
            };

            var responseEdited = await _httpClient.PutAsync(Uri, new StringContent(JsonConvert.SerializeObject(cInfoInserted), Encoding.UTF8, "application/json"));
            Assert.IsTrue(responseEdited.StatusCode == System.Net.HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task Update_WhenCalledAndDataIsValid_ReturnsEditedProductDTO()
        {
            var rand = new Random();
            var randomName = string.Join("", Enumerable.Repeat(0, 10).Select(n => (char)rand.Next(127)));

            NewProductDTO cInfo = new NewProductDTO()
            {
                Name = randomName,
                Description = new Random().Next(1000).ToString(),
                StatusId = 1,
                Price = new Random().Next(1000),
                Stock = 1
            };

            string Uri = BaseUri + "api/Products/";
            var response = await _httpClient.PostAsync(Uri, new StringContent(JsonConvert.SerializeObject(cInfo), Encoding.UTF8, "application/json"));
            var result = await response.Content.ReadAsStringAsync();
            var cOutputInfo = JsonConvert.DeserializeObject<ProductDTO>(result);

            EditProductDTO cInfoInserted = new EditProductDTO()
            {
                ProductId = cOutputInfo.ProductId,
                Name = "Edited",
                Description = cOutputInfo.Description,
                StatusId = 1,
                Price = cOutputInfo.Price,
                Stock = cOutputInfo.Stock
            };

            var responseEdited = await _httpClient.PutAsync(Uri, new StringContent(JsonConvert.SerializeObject(cInfoInserted), Encoding.UTF8, "application/json"));
            var cEditedInfo = JsonConvert.DeserializeObject<ProductDTO>(await responseEdited.Content.ReadAsStringAsync());
            Assert.IsTrue(responseEdited.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.IsInstanceOfType(cEditedInfo, typeof(ProductDTO));
        }

        [TestMethod]
        public async Task Delete_WhenCalledAndProductNoExists_ReturnsNotFound()
        {
            var rand = new Random();
            var randomName = string.Join("", Enumerable.Repeat(0, 10).Select(n => (char)rand.Next(127)));
            long deleteid = 0;

            string Uri = BaseUri + "api/Products/" + deleteid.ToString();
            var response = await _httpClient.DeleteAsync(Uri);
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.NotFound);
        }

        [TestMethod]
        public async Task Delete_WhenCalledAndProductExists_ReturnsOK()
        {
            var rand = new Random();
            var randomName = string.Join("", Enumerable.Repeat(0, 10).Select(n => (char)rand.Next(127)));

            NewProductDTO cInfo = new NewProductDTO()
            {
                Name = randomName,
                Description = new Random().Next(1000).ToString(),
                StatusId = 1,
                Price = new Random().Next(1000),
                Stock = 1
            };

            string Uri = BaseUri + "api/Products/";
            var response = await _httpClient.PostAsync(Uri, new StringContent(JsonConvert.SerializeObject(cInfo), Encoding.UTF8, "application/json"));
            var result = await response.Content.ReadAsStringAsync();
            var cOutputInfo = JsonConvert.DeserializeObject<ProductDTO>(result);
            Uri += cOutputInfo.ProductId.ToString();

            var responseDeleted = await _httpClient.DeleteAsync(Uri);
            //var cEditedInfo = JsonConvert.DeserializeObject<ProductDTO>(await response.Content.ReadAsStringAsync());
            Assert.IsTrue(responseDeleted.StatusCode == System.Net.HttpStatusCode.OK);
        }
    }
}
