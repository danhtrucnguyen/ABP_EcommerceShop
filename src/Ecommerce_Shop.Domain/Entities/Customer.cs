using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ecommerce_Shop.Entities
{
    public class Customer : AuditedAggregateRoot<Guid>
    {
        public string Name { get; private set; } = default!;
        public string Email { get; private set; } = default!;

        public string? Phone { get; set; }
        public string? Address { get; set; }

        private Customer() { }
        public Customer(Guid id, string name, string email, string phone, string address) : base(id)
        {
            Name = name;
            Email = email;
            Phone = phone;
            Address = address;
        }
    }
}
