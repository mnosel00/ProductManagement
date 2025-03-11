using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Domain.Interfaces
{
    public interface IBadWordsRepository
    {
        Task<IEnumerable<string>> GetBadWordsAsync();
    }
}
