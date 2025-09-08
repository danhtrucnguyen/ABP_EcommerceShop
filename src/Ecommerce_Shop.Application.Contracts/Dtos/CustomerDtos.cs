using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Ecommerce_Shop.Dtos
{
    public class CustomerDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }

    public class CreateUpdateCustomerDto
    {
        [Required, StringLength(128)]
        public string Name { get; set; } = default!;

        [Required, EmailAddress, StringLength(256)]
        public string Email { get; set; } = default!;

        [StringLength(32)]
        public string? Phone { get; set; }

        [StringLength(512)]
        public string? Address { get; set; }
    }
}
