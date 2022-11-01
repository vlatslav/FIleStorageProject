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
    public class FilesRepositoryTests
    {
        [TestCase(1)]
        [TestCase(2)]
        public async Task FilesRepository_GetByIdAsync_ReturnsSingleValue(int id)
        {
            using var context = new FileDBContext(UnitTestHelper.GetUnitTestDbOptions());

            var fileRepository = new FileRepository(context);

            var file = await fileRepository.GetById(id);

            var expected = ExpectedFiles.FirstOrDefault(x => x.FileId == id);

            Assert.That(file, Is.EqualTo(expected).Using(new FileEqualityComparer()), message: "GetByIdAsync method works incorrect");
        }
        [Test]
        public async Task FilesRepository_GetAllAsync_ReturnsAllValues()
        {
            using var context = new FileDBContext(UnitTestHelper.GetUnitTestDbOptions());

            var fileRepository = new FileRepository(context);

            var files = await fileRepository.GetAll();

            Assert.That(files, Is.EqualTo(ExpectedFiles).Using(new FileEqualityComparer()), message: "GetAllAsync method works incorrect");
        }
        [Test]
        public async Task FilesRepository_AddAsync_AddsValueToDatabase()
        {
            using var context = new FileDBContext(UnitTestHelper.GetUnitTestDbOptions());

            var fileRepository = new FileRepository(context);
            var file = new Files() { FileId = 3};

            await fileRepository.Add(file);
            await context.SaveChangesAsync();

            Assert.That(context.Files.Count(), Is.EqualTo(3), message: "AddAsync method works incorrect");
        }
        [Test]
        public async Task FilesRepository_DeleteByIdAsync_DeletesEntity()
        {
            using var context = new FileDBContext(UnitTestHelper.GetUnitTestDbOptions());

            var fileRepository = new FileRepository(context);

            await fileRepository.DeleteByIdAsync(1);
            await context.SaveChangesAsync();

            Assert.That(context.Files.Count(), Is.EqualTo(1), message: "DeleteByIdAsync works incorrect");
        }
        [Test]
        public async Task FilesRepository_Update_UpdatesEntity()
        {
            using var context = new FileDBContext(UnitTestHelper.GetUnitTestDbOptions());

            var fileRepository = new FileRepository(context);
            var file = new Files()
            {
                FileId = 1,
                FileName = "Update"
            };

            fileRepository.Update(file);
            await context.SaveChangesAsync();

            Assert.That(file, Is.EqualTo(new Files()
            {
                FileId = 1,
                FileName = "Update"
            }).Using(new FileEqualityComparer()), message: "Update method works incorrect");
        }
        [Test]
        public async Task FilesRepository_GetAllWithDetailsAsync_ReturnsWithIncludedEntities()
        {
            using var context = new FileDBContext(UnitTestHelper.GetUnitTestDbOptions());

            var fileRepository = new FileRepository(context);

            var files = await fileRepository.GetAllWithDetails();

            Assert.That(files,
                Is.EqualTo(ExpectedFiles).Using(new FileEqualityComparer()), message: "GetAllWithDetailsAsync method works incorrect");

            Assert.That(files.Select(i => i.Category).OrderBy(i => i.CategoryId),
                Is.EqualTo(ExpectedCutegories).Using(new CategoryEqualityComparer()), message: "GetAllWithDetailsAsync method doesnt't return included entities");
        }
        private static IEnumerable<Files> ExpectedFiles =>
            new[]
            {
                new Files(){CategoryId = 1, ContentType = "jpg", Date = new DateTime(2022,9,9), FileName = "File1", Description = "Desc1", Title = "Title1", FilePath = "C:\\Vluad\\BAL\\PL\\Files\\File_34.jpg", FileId = 1, UserId = "testUser" },
                new Files() { CategoryId = 2, ContentType = "jpg", Date = new DateTime(2022, 7, 1), FileName = "File2", Description = "Desc2", Title = "Title2", FilePath = "C:\\Vluad\\BAL\\PL\\Files\\File_44.jpg", FileId = 2, UserId = "testUser2" }
            };

        private static IEnumerable<Category> ExpectedCutegories =>
            new[]
            {
                new Category() { CategoryId = 1, CategoryName = "Games" },
                new Category() { CategoryId = 2, CategoryName = "Images" }
            };
    }
}
