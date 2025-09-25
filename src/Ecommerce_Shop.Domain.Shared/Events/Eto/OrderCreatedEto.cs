using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce_Shop.Events.Eto
{
    [Serializable]
    public class OrderCreatedEto
    {
        public Guid OrderId { get; set; }

        public Guid CustomerId { get; set; }

        public decimal TotalAmount { get; set; }
        public List<OrderItemEto> Items { get; set; } = new();
        public DateTime CreatedAt { get; set; }
    }

    [Serializable]
    public class OrderItemEto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
