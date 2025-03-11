using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.Interfaces
{
    public interface IProductBadWordsService
    {
        Task<bool> ContainsBadWordsAsync(string text);
    }
}
