using Microsoft.EntityFrameworkCore;
using ProductManagement.Domain.Interfaces;
using ProductManagement.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Infrastructure.Repositories
{
    public class BadWordRepository : IBadWordsRepository
    {
        private readonly ProductDbContext _context;

        public BadWordRepository(ProductDbContext productDbContext)
        {
            _context = productDbContext;
        }

        public async Task<IEnumerable<string>> GetBadWordsAsync() => 
            await _context.BadWords.Select(x => x.Word).ToListAsync();

    }
}
