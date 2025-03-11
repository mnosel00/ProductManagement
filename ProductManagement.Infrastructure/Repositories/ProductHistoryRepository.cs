using Microsoft.EntityFrameworkCore;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Interfaces;
using ProductManagement.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Infrastructure.Repositories
{
    public class ProductHistoryRepository : IProductHistoryRepository
    {
        private readonly ProductDbContext _context;
        public ProductHistoryRepository(ProductDbContext context)
        {
            _context = context;
        }
        public async Task AddHistoryAsync(ProductHistory history)
        {
            _context.ProductHistories.Add(history);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductHistory>> GetAllHistoryAsync(int productId) => 
            await _context.ProductHistories.Where(x => x.ProductId == productId).ToListAsync();

    }
}
