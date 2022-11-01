using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAL;
using BAL.Entity;
using BAL.Repositories;
using NUnit.Framework;

namespace Tests.DataTests
{
    [TestFixture]
    public class CategoryRepositoryTests
    {
        [TestCase(1)]
        [TestCase(2)]
        public async Task CategoryRepository_GetByIdAsync_ReturnsSingleValue(int id)
        {
            using var context = new FileDBContext(UnitTestHelper.GetUnitTestDbOptions());

            var categoryRepository = new CategoryRepository(context);

            var category = await categoryRepository.GetById(id);

            var expected = ExpectedCutegories.FirstOrDefault(x => x.CategoryId == id);

            Assert.That(category, Is.EqualTo(expected).Using(new CategoryEqualityComparer()), message: "GetByIdAsync method works incorrect");
        }


        [Test]
        public async Task CategoryRepository_GetAllAsync_ReturnsAllValues()
        {
            using var context = new FileDBContext(UnitTestHelper.GetUnitTestDbOptions());

            var categoryRepository = new CategoryRepository(context);

            var category = await categoryRepository.GetAll();

            Assert.That(category, Is.EqualTo(ExpectedCutegories).Using(new CategoryEqualityComparer()), message: "GetAllAsync method works incorrect");
        }
        [Test]
        public async Task CategoryRepository_AddAsync_AddsValueToDatabase()
        {
            using var context = new FileDBContext(UnitTestHelper.GetUnitTestDbOptions());

            var categoryRepository = new CategoryRepository(context);
            var category = new Category(){ CategoryId = 6, CategoryName = "Test"};

            await categoryRepository.Add(category);
            await context.SaveChangesAsync();

            Assert.That(context.Categories.Count(), Is.EqualTo(6), message: "AddAsync method works incorrect");
        }

        [Test]
        public async Task CategoryRepository_DeleteByIdAsync_DeletesEntity()
        {
            using var context = new FileDBContext(UnitTestHelper.GetUnitTestDbOptions());

            var categoryRepository = new CategoryRepository(context);

            await categoryRepository.DeleteByIdAsync(1);
            await context.SaveChangesAsync();

            Assert.That(context.Categories.Count(), Is.EqualTo(4), message: "DeleteByIdAsync works incorrect");
        }
        [Test]
        public async Task CategoryRepository_Update_UpdatesEntity()
        {
            using var context = new FileDBContext(UnitTestHelper.GetUnitTestDbOptions());

            var customerRepository = new CategoryRepository(context);
            var category = new Category()
            {
                CategoryId = 1,
                CategoryName = "UpdateTest"
            };

            customerRepository.Update(category);
            await context.SaveChangesAsync();

            Assert.That(category, Is.EqualTo(new Category()
            {
                CategoryId = 1,
                CategoryName = "UpdateTest"
            }).Using(new CategoryEqualityComparer()), message: "Update method works incorrect");
        }
        //private static IEnumerable<Files> ExpectedFiles =>
        //    new[]
        //    {
        //        new Files(){CategoryId = 1, ContentType = "jpg", Date = new DateTime(2022,9,9), FileName = "File1", Description = "Desc1", Title = "Title1", FilePath = "C:\\Vluad\\BAL\\PL\\Files\\File_34.jpg", FileId = 1, UserId = "testUser" },
        //        new Files() { CategoryId = 2, ContentType = "jpg", Date = new DateTime(2022, 7, 1), FileName = "File2", Description = "Desc2", Title = "Title2", FilePath = "C:\\Vluad\\BAL\\PL\\Files\\File_44.jpg", FileId = 2, UserId = "testUser2" }
        //    };

        private static IEnumerable<Category> ExpectedCutegories =>
            new[]
            {
                new Category() { CategoryId = 1, CategoryName = "Games" },
                new Category() { CategoryId = 2, CategoryName = "Images" },
                new Category() { CategoryId = 3, CategoryName = "Videos" },
                new Category() { CategoryId = 4, CategoryName = "Books" },
                new Category() { CategoryId = 5, CategoryName = "Scripts" }
            };

    }
}
