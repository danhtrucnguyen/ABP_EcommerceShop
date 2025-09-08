using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace Ecommerce_Shop.Entities
{
    public class Category : Entity<Guid>
    {
        public string Name { get; private set; } = default!;

        public string? Description { get; set; }
        private Category() { }
        public Category(Guid id, string name, string? description = null) : base(id)
        {
            Name = name;
            Description = description;
        }
    }
}
