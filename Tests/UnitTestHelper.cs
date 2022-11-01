using System;
using System.Collections.Generic;
using AutoMapper;
using BAL;
using BAL.Entity;
using BAL.Entity.Auth;
using BusinessLogicLayer;
using Microsoft.EntityFrameworkCore;

namespace Tests
{
    internal static class UnitTestHelper
    {
        public static DbContextOptions<FileDBContext> GetUnitTestDbOptions()
        {
            var options = new DbContextOptionsBuilder<FileDBContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;


            using (var context = new FileDBContext(options))
            {
                SeedData(context);
            }

            return options;
        }

        public static IMapper CreateMapperProfile()
        {
            var myProfile = new AutoMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));

            return new Mapper(configuration);
        }

        public static void SeedData(FileDBContext context)
        {
            
            context.Categories.AddRange(new Category() { CategoryId = 1, CategoryName = "Games" },
                new Category() { CategoryId = 2, CategoryName = "Images" },
                new Category() { CategoryId = 3, CategoryName = "Videos" },
                new Category() { CategoryId = 4, CategoryName = "Books" },
                new Category() { CategoryId = 5, CategoryName = "Scripts" });
            context.Users.AddRange(
                new User() { Id = "testUser", UserName = "test" },
                new User() { Id = "testUser2", UserName = "test2" },
                new User() { Id = "testUser3", UserName = "test3" });
            context.Files.AddRange(
                new Files(){CategoryId = 1, ContentType = "jpg", Date = new DateTime(2022,9,9), FileName = "File1", Description = "Desc1", Title = "Title1", FilePath = "C:\\Vluad\\BAL\\PL\\Files\\File_34.jpg", FileId = 1, UserId = "testUser" },
                            new Files() { CategoryId = 2, ContentType = "jpg", Date = new DateTime(2022, 7, 1), FileName = "File2", Description = "Desc2", Title = "Title2", FilePath = "C:\\Vluad\\BAL\\PL\\Files\\File_44.jpg", FileId = 2, UserId = "testUser2" });
            context.SaveChanges();
        }
    }

}
