using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAL.Entity;
using BAL.Interfaces;
using BusinessLogicLayer.Models;
using BusinessLogicLayer.Services;
using BusinessLogicLayer.Validation;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Tests.BusinessTests
{
    public class CategoryServiceTests
    {
        [Test]
        public async Task CategoryService_GetAll_ReturnsAllCustomers()
        {
            //arrange
            var expected = GetTestCategories;
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mapper = UnitTestHelper.CreateMapperProfile();
            var expectedres = mapper.Map<IEnumerable<CategoryModel>>(expected);
            mockUnitOfWork
                .Setup(x => x.CategoryRepository.GetAll())
                .ReturnsAsync(GetTestCategories.AsEnumerable());

            var categoryService = new CategoryService(mockUnitOfWork.Object, mapper);

            //act
            var actual = await categoryService.GetAllAsync();

            //assert
            actual.Should().BeEquivalentTo(expectedres);

        }
        [Test]
        public async Task CategoryService_GetById_ReturnsModel()
        {
            //arrange
            var expected = GetTestCategories;
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mapper = UnitTestHelper.CreateMapperProfile();
            var expectedres = mapper.Map<IEnumerable<CategoryModel>>(expected);
            mockUnitOfWork
                .Setup(m => m.CategoryRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync(GetTestCategories.First());
            var categoryService = new CategoryService(mockUnitOfWork.Object, mapper);

            //act
            var actual = await categoryService.GetByIdAsync(1);

            //assert
            actual.Should().BeEquivalentTo(expectedres.First());
        }

        [Test]
        public async Task CategoryService_AddAsync_AddsModel()
        {
            //arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(m => m.CategoryRepository.Add(It.IsAny<Category>()));
            var mapper = UnitTestHelper.CreateMapperProfile();
            var categoryService = new CategoryService(mockUnitOfWork.Object, mapper);
            var category = GetTestCategories.First();
            
            //act
            await categoryService.AddAsync(mapper.Map<CategoryModel>(category));

            //assert
            mockUnitOfWork.Verify(x => x.CategoryRepository.Add(It.Is<Category>(x =>
                x.CategoryId == category.CategoryId && x.CategoryName == category.CategoryName)), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
        [Test]
        public async Task CategoryService_AddAsync_ThrowsExceptionWithEmptyName()
        {
            //arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(m => m.CategoryRepository.Add(It.IsAny<Category>()));
            var mapper = UnitTestHelper.CreateMapperProfile();
            var categoryService = new CategoryService(mockUnitOfWork.Object, mapper);
            var category = GetTestCategories.First();
            category.CategoryName = string.Empty;

            //act
            Func<Task> act = async () => await categoryService.AddAsync(mapper.Map<CategoryModel>(category));

            //assert
            await act.Should().ThrowAsync<FileExcpetion>();
        }
        [Test]
        public async Task CategoryService_AddAsync_ThrowsExceptionWithNullObject()
        {
            //arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(m => m.CategoryRepository.Add(It.IsAny<Category>()));
            var mapper = UnitTestHelper.CreateMapperProfile();
            var categoryService = new CategoryService(mockUnitOfWork.Object, mapper);

            //act
            Func<Task> act = async () => await categoryService.AddAsync(null);

            //assert
            await act.Should().ThrowAsync<FileExcpetion>();
        }
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(100)]
        public async Task CategoryService_DeleteAsync_DeletesCategory(int id)
        {
            //arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(m => m.CategoryRepository.DeleteByIdAsync(It.IsAny<int>()));
            var mapper = UnitTestHelper.CreateMapperProfile();
            var categoryService = new CategoryService(mockUnitOfWork.Object, mapper);
            //act
            await categoryService.DeleteAsync(id);

            //assert
            mockUnitOfWork.Verify(x => x.CategoryRepository.DeleteByIdAsync(id), Times.Once());
            mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once());
        }
        [Test]
        public async Task CategoryService_UpdateAsync_UpdatesCustomer()
        {
            //arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(m => m.CategoryRepository.Update(It.IsAny<Category>()));
            mockUnitOfWork.Setup(m => m.FileRepository.Update(It.IsAny<Files>()));

            var mapper = UnitTestHelper.CreateMapperProfile();

            var categoryService = new CategoryService(mockUnitOfWork.Object, mapper);
            var category = GetTestCategories.First();

            //act
            await categoryService.UpdateAsync(mapper.Map<CategoryModel>(category));

            //assert
            mockUnitOfWork.Verify(x => x.CategoryRepository.Update(It.Is<Category>(x =>
                x.CategoryId == category.CategoryId && x.CategoryName == category.CategoryName)), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        #region TestData
        public List<Category> GetTestCategories =>
            new List<Category>()
            {
                new Category() { CategoryId = 1, CategoryName = "Games" },
                new Category() { CategoryId = 2, CategoryName = "Images" },
                new Category() { CategoryId = 3, CategoryName = "Videos" },
                new Category() { CategoryId = 4, CategoryName = "Books" },
                new Category() { CategoryId = 5, CategoryName = "Scripts" }
            };
        #endregion

    }
}
