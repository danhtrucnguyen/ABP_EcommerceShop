using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ecommerce_Shop.Entities
{
    public class Review : AuditedAggregateRoot<Guid>
    {
        public Guid ProductId { get; set; }
        public Guid CustomerId { get; set; }
        public byte Rating { get; set; }   // 1..5
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsApproved { get; set; }
    }
}
