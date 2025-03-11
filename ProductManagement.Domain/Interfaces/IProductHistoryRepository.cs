using ProductManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Domain.Interfaces
{
    public interface IProductHistoryRepository
    {
        Task<IEnumerable<ProductHistory>> GetAllHistoryAsync(int productId);
        Task AddHistoryAsync(ProductHistory history);
    }
}
