using System;
using System.Collections.Generic;

namespace Ecommerce_Shop.Samples
{
    public class CustomerOrdersExplicitResultDto
    {
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int TotalOrders { get; set; }
        public int TotalItems { get; set; }
        public List<OrderBriefDto> Orders { get; set; } = new();
    }

    public class OrderBriefDto
    {
        public Guid OrderId { get; set; }
        public int ItemCount { get; set; }
        public List<ItemBriefDto> Items { get; set; } = new();
    }

    public class ItemBriefDto
    {
        public Guid OrderItemId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }
}
