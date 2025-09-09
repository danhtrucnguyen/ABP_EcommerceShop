using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ecommerce_Shop.Entities
{
    public class OrderItem : AuditedAggregateRoot<Guid>
    {
        public Guid OrderId { get; set; }
        public Order? Order { get; set; }

        public Guid ProductId { get; set; }
        public Product? Product { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        private OrderItem() { }
        public OrderItem(Guid id) : base(id) { }
    }
}
