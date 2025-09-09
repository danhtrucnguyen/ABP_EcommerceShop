using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Ecommerce_Shop.Dtos
{

    public class OrderDto : AuditedEntityDto<Guid>
    {
        public Guid CustomerId { get; set; }
        public string? CustomerName { get; set; }  
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = default!;
        public List<OrderItemDto> Items { get; set; } = new();
    }

    public class OrderItemDto : AuditedEntityDto<Guid>
    {
        public Guid ProductId { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
    }

    public class CreateOrderDto
    {
        [Required] public Guid CustomerId { get; set; }
        [MinLength(1)] public List<CreateOrderItemDto> Items { get; set; } = new();
    }

    public class CreateOrderItemDto
    {
        [Required] public Guid ProductId { get; set; }
        [Range(1, int.MaxValue)] public int Quantity { get; set; }

        [Range(0, double.MaxValue)]
        public decimal UnitPrice { get; set; }
    }

    public class LookupDto<TKey>
    {
        public TKey Id { get; set; }
        public string DisplayName { get; set; } = default!;
    }

    public class OrderFormLookupsDto
    {
        public List<LookupDto<Guid>> Customers { get; set; } = new();
        public List<LookupDto<Guid>> Products { get; set; } = new();
    }

}
