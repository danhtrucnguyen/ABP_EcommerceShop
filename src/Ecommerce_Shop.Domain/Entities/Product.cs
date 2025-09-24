using Ecommerce_Shop.Domain.Events;
using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ecommerce_Shop.Entities
{
    public class Product : FullAuditedAggregateRoot<Guid>  
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

        public void ChangePrice(decimal newPrice)
        {
            if (newPrice < 0)
                throw new ArgumentOutOfRangeException(nameof(newPrice));

            if (Price == newPrice)
                return;

            var old = Price;
            Price = newPrice;

            AddLocalEvent(new ProductPriceChangedEvent(Id, old, newPrice));
        }
    }
}
