using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Ecommerce_Shop.Dtos
{
    public class ProductDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public Guid? CategoryId { get; set; }
    }

    public class CreateUpdateProductDto
    {
        [Required(ErrorMessage = "Tên sản phẩm không được bỏ trống")]
        [StringLength(128, ErrorMessage = "Tên sản phẩm tối đa 128 ký tự")]
        public string Name { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        public decimal Price { get; set; }

        public string Description { get; set; }

        public Guid? CategoryId { get; set; }
    }
}
