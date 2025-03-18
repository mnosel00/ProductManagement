using Microsoft.EntityFrameworkCore;
using ProductManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Infrastructure.Data
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductHistory> ProductHistories { get; set; }
        public DbSet<BadWord> BadWords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<ProductHistory>()
                .HasKey(ph => ph.Id);

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Name)
                .IsUnique();
        }
    }
}
