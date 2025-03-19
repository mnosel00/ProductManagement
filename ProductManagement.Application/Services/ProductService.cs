using ProductManagement.Application.DTOs;
using ProductManagement.Application.Interfaces;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Enums;
using ProductManagement.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IProductHistoryService _productHistoryService;
        private readonly IProductBadWordsService _productBadWordsService;

        public ProductService(IProductRepository repository, IProductHistoryService productHistoryService, IProductBadWordsService productBadWordsService)
        {
            _repository = repository;
            _productHistoryService = productHistoryService;
            _productBadWordsService = productBadWordsService;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync() => await _repository.GetAllAsync();

        public async Task<Product?> GetProductByIdAsync(int id) => await _repository.GetByIdAsync(id);

        public async Task AddProductAsync(ProductDto productDto)
        {
            if (await _productBadWordsService.ContainsBadWordsAsync(productDto.Name))
                throw new Exception("Nazwa produktu zawiera złe słowa");

            if (await _repository.ExistsByNameAsync(productDto.Name))
                throw new Exception("Produkt o podanej nazwie już istnieje");

            var product = new Product(productDto.Name, productDto.Category, productDto.Price, productDto.Quantity);
            await _repository.AddAsync(product);
        }

        public async Task UpdateProductAsync(Product product)
        {
            var existingProduct = await _repository.GetByIdAsync(product.Id);
            if (existingProduct == null)
                throw new Exception("Product not found");

            await _productHistoryService.SaveProductHistoryAsync(existingProduct);

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Quantity = product.Quantity;
            existingProduct.Category = product.Category;

            await _repository.UpdateAsync(existingProduct);
        }

        public async Task DeleteProductAsync(int id) => await _repository.DeleteAsync(id);
    }
}
