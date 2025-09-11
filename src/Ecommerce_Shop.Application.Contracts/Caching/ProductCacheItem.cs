using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce_Shop.Caching
{
    public class ProductCacheItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public Guid? CategoryId { get; set; }
    }
}
