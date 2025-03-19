using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using FluentAssertions;
using ProductManagement.API.Controllers;
using ProductManagement.Application.Interfaces;
using ProductManagement.Application.Services;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Interfaces;
using ProductManagement.Infrastructure.Data;
using ProductManagement.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductManagement.Application.DTOs;
using ProductManagement.Domain.Enums;

namespace ProductManagement.Test
{
    public class UnitTest1 : IDisposable
    {
        private ProductsController _controller;
        private ProductDbContext _context;
        private IProductService _productService;

        [SetUp]
        public void Setup()
        {
            _context = CreateInMemoryDbContext();
            var repo = new ProductRepository(_context);
            var historyMock = Mock.Of<IProductHistoryService>();
            var badWordsMock = Mock.Of<IProductBadWordsService>();

            _productService = new ProductService(repo, historyMock, badWordsMock);
            _controller = new ProductsController(_productService);
            SeedDatabase();
        }

        private static ProductDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ProductDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ProductDbContext(options);
        }

        private void SeedDatabase()
        {
            _context.Products.AddRange(new List<Product>
            {
                new Product("Product 1", ProductCategory.Electronics, 10, 5) { Id = 1 },
                new Product("Product 2", ProductCategory.Books, 20, 10) { Id = 2 }
            });
            _context.SaveChanges();
        }

        [Test]
        public async Task GetAllProducts_ShouldReturnAllProducts()
        {
            var expectedCount = await _context.Products.CountAsync();

            var result = await _controller.GetAll() as OkObjectResult;
            var products = result?.Value as IEnumerable<Product>;

            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(200);
            products.Should().NotBeNull();
            products!.Count().Should().Be(expectedCount);
        }

        [Test]
        public async Task GetById_ShouldReturnProduct()
        {
            var expectedProduct = await _context.Products.FindAsync(1);

            var result = await _controller.GetById(1) as OkObjectResult;
            var product = result?.Value as Product;

            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(200);
            product.Should().NotBeNull();
            product!.Id.Should().Be(expectedProduct!.Id);
            product.Name.Should().Be(expectedProduct.Name);
        }

        [Test]
        public async Task Create_ShouldCreateProduct()
        {
            var productDto = new ProductDto { Name = "Product 3", Category = ProductCategory.Clothes, Price = 30, Quantity = 15 };

            var result = await _controller.Create(productDto) as CreatedAtActionResult;
            var createdProduct = result?.Value as ProductDto;

            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(201);
            createdProduct.Should().NotBeNull();
            createdProduct!.Name.Should().Be(productDto.Name);
        }

        [Test]
        public async Task Update_ShouldUpdateProduct()
        {
            var updatedProduct = new Product("Product 1 Updated", ProductCategory.Electronics, 15, 10) { Id = 1 };

            var result = await _controller.Update(1, updatedProduct) as NoContentResult;

            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(204);

            var productInDb = await _context.Products.FindAsync(1);
            productInDb.Should().NotBeNull();
            productInDb!.Name.Should().Be(updatedProduct.Name);
            productInDb.Price.Should().Be(updatedProduct.Price);
            productInDb.Quantity.Should().Be(updatedProduct.Quantity);
        }

        [Test]
        public async Task Delete_ShouldDeleteProduct()
        {
            (await _context.Products.FindAsync(1)).Should().NotBeNull();

            var result = await _controller.Delete(1) as NoContentResult;

            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(204);
            (await _context.Products.FindAsync(1)).Should().BeNull();
        }

        [TearDown]
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

