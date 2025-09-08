using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Ecommerce_Shop.Dtos
{
    public class CategoryDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }

    public class CreateUpdateCategoryDto
    {
        [Required]
        [StringLength(128)]
        public string Name { get; set; } = default!;

        [StringLength(1024)]
        public string? Description { get; set; }
    }
}
