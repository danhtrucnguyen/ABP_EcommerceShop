using System;

namespace Ecommerce_Shop.Domain.Events
{

    public sealed class ProductPriceChangedEvent
    {
        public Guid ProductId { get; }
        public decimal OldPrice { get; }
        public decimal NewPrice { get; }
        public DateTime ChangeTime { get; }

        public ProductPriceChangedEvent(Guid productId, decimal oldPrice, decimal newPrice)
        {
            ProductId = productId;
            OldPrice = oldPrice;
            NewPrice = newPrice;
            ChangeTime = DateTime.UtcNow;
        }
    }
}
