using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using ProductManagement.API.Controllers;
using ProductManagement.Application.Interfaces;
using ProductManagement.Application.Services;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Interfaces;
using ProductManagement.Infrastructure.Data;
using NUnit.Framework;
using FluentAssertions;

namespace ProductManagement.Test
{
    public class Tests : IDisposable
    {
        private ProductsController _controller;
        private ProductDbContext _context;
        private IProductService _productService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ProductDbContext>()
                .UseInMemoryDatabase(databaseName: "ProductDbTest")
                .Options;

            _context = new ProductDbContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            var repoMock = new Mock<IProductRepository>().Object;
            var historyMock = new Mock<IProductHistoryService>().Object;
            var badWordsMock = new Mock<IProductBadWordsService>().Object;

            _productService = new ProductService(repoMock, historyMock, badWordsMock);
            _controller = new ProductsController(_productService);
            SeedDatabase();
        }

        private void SeedDatabase()
        {
            _context.Products.AddRange(
                new Product { Id = 1, Name = "Product 1", Price = 10, Quantity = 5 },
                new Product { Id = 2, Name = "Product 2", Price = 20, Quantity = 10 }
                );
            _context.SaveChanges();
        }

        [Test]
        public async Task GetAllProducts_ShouldReturnAllProducts()
        {
            var result = await _controller.GetAll() as OkObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));

            var products = result.Value as IEnumerable<Product>; //as List<Product>;
            Assert.That(products, Is.Not.Null);
            Assert.That(products.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetById_ShouldReturnProduct()
        {
            var result = await _controller.GetById(1) as OkObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));

            var product = result.Value as Product;

            Assert.That(product, Is.Not.Null);
            Assert.That(product.Id, Is.EqualTo(1));
            Assert.That(product.Name, Is.EqualTo("Product 1"));
        }

        [Test]
        public async Task Create_ShouldCreateProduct()
        {
            var product = new Product { Name = "Product 3", Price = 30, Quantity = 15 };

            var result = await _controller.Create(product) as CreatedAtActionResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(201));

            var createdProduct = result.Value as Product;
            Assert.That(createdProduct, Is.Not.Null);
            Assert.That(createdProduct.Id, Is.GreaterThan(0));
            Assert.That(createdProduct.Name, Is.EqualTo("Product 3"));
        }

        [Test]
        public async Task Update_ShouldUpdateProduct()
        {
            var product = new Product { Id = 1, Name = "Product 1 Updated", Price = 15, Quantity = 10 };

            var result = await _controller.Update(1, product) as NoContentResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(204));

            var updatedProduct = await _context.Products.FindAsync(1);
            Assert.That(updatedProduct, Is.Not.Null);
            Assert.That(updatedProduct.Name, Is.EqualTo("Product 1 Updated"));
        }

        [Test]
        public async Task Delete_ShouldDeleteProduct()
        {
            var result = await _controller.Delete(1) as NoContentResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(204));

            var deletedProduct = await _context.Products.FindAsync(1);
            Assert.That(deletedProduct, Is.Null);
        }

  
        [TearDown]
        public void Dispose()
        {
         _context.Dispose();
        }
        
    }

}