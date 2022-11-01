using BusinessLogicLayer.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using FluentAssertions;

namespace Tests.IntegrationTests
{
    public class FileIntegrationTest
    {
        private CustomWebApplicationFactory _factory;
        private HttpClient _client;
        private const string RequestUri = "api/file/";

        [SetUp]
        public void Init()
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
        }











        //private static readonly IEnumerable<FileModel> ExpectedCategoryModels =
        //    new List<FileModel>()
        //    {
        //        new File() { CategoryId = 1,CategoryName = "Games"},
        //        new CategoryModel() { CategoryId = 2, CategoryName = "Images"},
        //        new CategoryModel() { CategoryId = 3, CategoryName = "Videos"},
        //        new CategoryModel() { CategoryId = 4, CategoryName = "Books"},
        //        new CategoryModel() { CategoryId = 5, CategoryName = "Scripts"}
        //    };


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
