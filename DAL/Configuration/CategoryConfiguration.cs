using System;
using System.Collections.Generic;
using System.Text;
using BAL.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BAL.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(
                new Category() { CategoryId = 1,CategoryName = "Games"},
                new Category() { CategoryId = 2, CategoryName = "Images"},
                new Category() { CategoryId = 3, CategoryName = "Videos"},
                new Category() { CategoryId = 4, CategoryName = "Books"},
                new Category() { CategoryId = 5, CategoryName = "Scripts"}
            );
        }
    }
}
