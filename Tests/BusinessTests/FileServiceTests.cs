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
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Moq;
using NUnit.Framework;

namespace Tests.BusinessTests
{
    public class FileServiceTests
    {
        [Test]
        public async Task FileService_GetAll_ReturnsAllCustomers()
        {
            //arrange
            var expected = GetTestFiles;

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mapper = UnitTestHelper.CreateMapperProfile();
            var expectedResults = mapper.Map<IEnumerable<FileModel>>(expected);
            mockUnitOfWork
                .Setup(x => x.FileRepository.GetAllWithDetails())
                .ReturnsAsync(GetTestFiles.AsEnumerable());

            var fileService = new FileService(mockUnitOfWork.Object, mapper, null);

            //act
            var actual = await fileService.GetAllAsync();
            //assert
            actual.Should().BeEquivalentTo(expectedResults);
            
        }
        [Test]
        public async Task FileService_GetById_ReturnsModel()
        {
            //arrange
            var expected = GetTestFiles;
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mapper = UnitTestHelper.CreateMapperProfile();
            var expectedResult = mapper.Map<IEnumerable<FileModel>>(expected);

            mockUnitOfWork
                .Setup(m => m.FileRepository.GetById(It.IsAny<int>()))
                .ReturnsAsync(GetTestFiles.First());
            var fileService = new FileService(mockUnitOfWork.Object, mapper, null);

            //act
            var actual = await fileService.GetByIdAsync(1);

            //assert
            actual.Should().BeEquivalentTo(expectedResult.First());
        }
        [Test]
        public async Task CustomerService_AddAsync_AddsModel()
        {
            //arrange

            //var mockEnvironment = new Mock<IHostingEnvironment>();
            //mockEnvironment
            //    .Setup(m => m.EnvironmentName)
            //    .Returns("Hosting:UnitTestEnvironment");

            var mapper = UnitTestHelper.CreateMapperProfile();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(m => m.FileRepository.Add(It.IsAny<Files>()));

            var fileService = new FileService(mockUnitOfWork.Object, mapper, null);
            var file = GetTestFiles.First();

            //act
            await fileService.AddAsync(mapper.Map<FileModel>(file));

            //assert
            mockUnitOfWork.Verify(x => x.FileRepository.Add(It.Is<Files>(x =>
                x.FileId == file.FileId && x.Date == file.Date &&
                x.ContentType == file.ContentType && x.FileName == file.FileName &&
                x.FilePath == file.FilePath)), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task CustomerService_AddAsync_ThrowsMarketExceptionWithEmptyName()
        {
            //arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mapper = UnitTestHelper.CreateMapperProfile();
            mockUnitOfWork.Setup(m => m.FileRepository.Add(It.IsAny<Files>()));

            var fileService = new FileService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile(), null);
            var file = GetTestFiles.First();
            file.FileName = string.Empty;

            //act
            Func<Task> act = async () => await fileService.AddAsync(mapper.Map<FileModel>(file));

            //assert
            await act.Should().ThrowAsync<FileExcpetion>();
        }

        [Test]
        public async Task CustomerService_AddAsync_ThrowsMarketExceptionWithNullObject()
        {
            //arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mapper = UnitTestHelper.CreateMapperProfile();
            mockUnitOfWork.Setup(m => m.FileRepository.Add(It.IsAny<Files>()));

            var fileService = new FileService(mockUnitOfWork.Object, mapper, null);

            //act
            Func<Task> act = async () => await fileService.AddAsync(null);

            //assert
            await act.Should().ThrowAsync<FileExcpetion>();
        }
        [TestCase(1)]
        [TestCase(2)]
        public async Task ProductService_DeleteAsync_DeletesProduct(int id)
        {
            //arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(m => m.FileRepository.DeleteByIdAsync(It.IsAny<int>()));

            var fileService = new FileService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile(), null);
            //act
            await fileService.DeleteAsync(id);

            //assert
            mockUnitOfWork.Verify(x => x.FileRepository.DeleteByIdAsync(id), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
        [Test]
        public async Task ProductService_UpdateAsync_UpdatesProduct()
        {
            //arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mapper = UnitTestHelper.CreateMapperProfile();
            mockUnitOfWork.Setup(m => m.FileRepository.Update(It.IsAny<Files>()));

            var fileService = new FileService(mockUnitOfWork.Object, mapper, null);
            var file = new FileModel() { FileId = 3, FileName = "Update", CategoryId = 4, ContentType = "jpg" };

            //act
            await fileService.UpdateAsync(file);

            //assert
            mockUnitOfWork.Verify(x => x.FileRepository.Update(It.Is<Files>(c => 
                c.FileId == file.FileId 
                && c.FileName == file.FileName 
                && c.ContentType == file.ContentType 
                && c.CategoryId == file.CategoryId)), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
        [Test]
        public async Task ProductService_UpdateAsync_ThrowsMarketExceptionsWithEmptyName()
        {
            //arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(m => m.FileRepository.Update(It.IsAny<Files>()));
            var mapper = UnitTestHelper.CreateMapperProfile();
            var fileService = new FileService(mockUnitOfWork.Object, mapper, null);
            var file = new FileModel() { FileId = 3, FileName = "", CategoryId = 4};

            //act
            Func<Task> act = async () => await fileService.UpdateAsync(file);

            //assert
            await act.Should().ThrowAsync<FileExcpetion>();
        }


        #region TestData
        public List<Files> GetTestFiles =>
            new List<Files>()
            {
                new Files(){CategoryId = 1, ContentType = "jpg", Date = new DateTime(2022,9,9), FileName = "File1", Description = "Desc1", Title = "Title1", FilePath = "C:\\Vluad\\BAL\\PL\\Files\\File_34.jpg", FileId = 1, UserId = "testUser" },
                new Files() {CategoryId = 2, ContentType = "jpg", Date = new DateTime(2022, 7, 1), FileName = "File2", Description = "Desc2", Title = "Title2", FilePath = "C:\\Vluad\\BAL\\PL\\Files\\File_44.jpg", FileId = 2, UserId = "testUser2" }
            };
        #endregion
    }
}
