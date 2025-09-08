using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ecommerce_Shop.Entities
{
    public class Product : AuditedAggregateRoot<Guid>
    {
        public string Name { get; private set; } = default!;
        public string? Description { get; private set; }
        public decimal Price { get; private set; }
        public Guid? CategoryId { get; set; }

        public Category? Category { get; set; }

        private Product() { }
        public Product(Guid id, string name, decimal price, Guid? categoryId = null) : base(id)
        {
            Name = name;
            Price = price;
            CategoryId = categoryId;
        }

        public void Update(string name, decimal price, string? description, Guid? categoryId)
        {
            Name = name;
            Price = price;
            Description = description;
            CategoryId = categoryId;
        }
    }
}
