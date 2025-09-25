using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce_Shop.Entities
{
    public sealed class OrderPaidEvent
    {
        public Guid OrderId { get; }
        public decimal TotalAmount { get; }
        public DateTime PaidAt { get; }

        public OrderPaidEvent(Guid orderId, decimal totalAmount, DateTime paidAt)
        {
            OrderId = orderId;
            TotalAmount = totalAmount;
            PaidAt = paidAt;
        }
    }
}
