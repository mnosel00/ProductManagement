using ProductManagement.Application.Interfaces;
using ProductManagement.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.Services
{
    public class ProductBadWordsService : IProductBadWordsService
    {
        private readonly IBadWordsRepository _badWordsRepository;

        public ProductBadWordsService(IBadWordsRepository badWordsRepository)
        {
            _badWordsRepository = badWordsRepository;
        }
        public async Task<bool> ContainsBadWordsAsync(string text)
        {
            var badWords = _badWordsRepository.GetBadWordsAsync().Result;
            return badWords.Any(word => text.Contains(word, StringComparison.OrdinalIgnoreCase));

        }
    }
}
