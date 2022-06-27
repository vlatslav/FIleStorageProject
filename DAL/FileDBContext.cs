using System;
using System.Collections.Generic;
using System.Text;
using BAL.Configuration;
using BAL.Entity;
using BAL.Entity.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BAL
{
    public class FileDBContext : IdentityDbContext<User>
    {

        //public FileDBContext()
        //{

        //}
        public FileDBContext(DbContextOptions<FileDBContext> options) : base(options)
        {
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.EnableSensitiveDataLogging();
        //    if (!optionsBuilder.IsConfigured)
        //        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=FileDb;Trusted_Connection=True;");
        //}

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            builder.Entity<User>()
                .HasMany(x => x.Files)
                .WithOne(x => x.User);
            builder.Entity<Category>()
                .HasMany(x => x.Files)
                .WithOne(x => x.Category);
            builder.ApplyConfiguration(new RoleConfiguration());
            builder.ApplyConfiguration(new CategoryConfiguration());
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Files> Files { get; set; }
    }
}
