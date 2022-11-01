using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicLayer.Models;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Tests.IntegrationTests
{
    public class CategoryIntegrationTest
    {
        private CustomWebApplicationFactory _factory;
        private HttpClient _client;
        private const string RequestUri = "api/category/";

        [SetUp]
        public void Init()
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
        }

        [Test]
        public async Task CategoryController_GetAll_ReturnsAllFromDb()
        {
            //arrange
            var expected = ExpectedCategoryModels.ToList();

            // act
            var httpResponse = await _client.GetAsync(RequestUri);

            // assert
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<CategoryModel>>(stringResponse).ToList();

            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task CategoryController_GetCategoryById_ReturnsFromDb()
        {
            //arrange
            var expected = ExpectedCategoryModels.ToList().First();
            var categoryId = 1;
            // act
            var httpResponse = await _client.GetAsync(RequestUri+ categoryId);

            // assert
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<CategoryModel>(stringResponse);

            actual.Should().BeEquivalentTo(expected);
        }


        //[Test]
        //public async Task CategoryController_AddCategory_Success()
        //{
        //    //arrange
        //    var categoryModel = new CategoryModel
        //    {
        //        CategoryId = 6,
        //        CategoryName = "Memes"
        //    };
        //    var content = new StringContent(JsonConvert.SerializeObject(categoryModel), Encoding.UTF8, "application/json");
        //    // act
        //    var httpResponse = await _client.PostAsync(RequestUri, content);

        //    // assert
        //    httpResponse.EnsureSuccessStatusCode();
        //    Assert.Equals(HttpStatusCode.OK, httpResponse.StatusCode);
        //}







        private static readonly IEnumerable<CategoryModel> ExpectedCategoryModels =
            new List<CategoryModel>()
            {
                new CategoryModel() { CategoryId = 1,CategoryName = "Games"},
                new CategoryModel() { CategoryId = 2, CategoryName = "Images"},
                new CategoryModel() { CategoryId = 3, CategoryName = "Videos"},
                new CategoryModel() { CategoryId = 4, CategoryName = "Books"},
                new CategoryModel() { CategoryId = 5, CategoryName = "Scripts"}
            };


        #region  helpers

        private async Task CheckExceptionWhileAddNewModel(CategoryModel customer)
        {
            var context = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync(RequestUri, context);

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        private async Task CheckExceptionWhileUpdateModel(CategoryModel customer)
        {
            var context = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PutAsync(RequestUri + customer.CategoryId, context);

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        #endregion

        [TearDown]
        public void TearDown()
        {
            _factory.Dispose();
            _client.Dispose();
        }


    }
}
