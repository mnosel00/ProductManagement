using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Domain.Entities
{
    public class BadWord
    {
        public int Id { get; set; }
        public string Word { get; set; } = string.Empty;
    }
}
