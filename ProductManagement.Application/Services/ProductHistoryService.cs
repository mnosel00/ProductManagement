using ProductManagement.Application.Interfaces;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.Services
{
    public class ProductHistoryService : IProductHistoryService
    {
        private readonly IProductHistoryRepository _repository;

        public ProductHistoryService(IProductHistoryRepository productHistoryRepository)
        {
            _repository = productHistoryRepository;
        }

        public Task<IEnumerable<ProductHistory>> GetProductHistoryAsync(int productId) => _repository.GetAllHistoryAsync(productId);

        public async Task SaveProductHistoryAsync(Product product)
        {
            var hisotry = new ProductHistory
            {
                ProductId = product.Id,
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity,
            };

            await _repository.AddHistoryAsync(hisotry);
        }
    }
}
