﻿using ProductManagement.Application.Interfaces;
using ProductManagement.Domain.Entities;
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

        public ProductService(IProductRepository repository, IProductHistoryService productHistoryService)
        {
            _repository = repository;
            _productHistoryService = productHistoryService;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync() => await _repository.GetAllAsync();

        public async Task<Product?> GetProductByIdAsync(int id) => await _repository.GetByIdAsync(id);

        public async Task AddProductAsync(Product product) => await _repository.AddAsync(product);

        public async Task UpdateProductAsync(Product product)
        {
            var existingProduct = await _repository.GetByIdAsync(product.Id);
            if (existingProduct == null)
                throw new Exception("Product not found");

            await _productHistoryService.SaveProductHistoryAsync(existingProduct);

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Quantity = product.Quantity;

            await _repository.UpdateAsync(existingProduct);
        }

        public async Task DeleteProductAsync(int id) => await _repository.DeleteAsync(id);
    }
}
