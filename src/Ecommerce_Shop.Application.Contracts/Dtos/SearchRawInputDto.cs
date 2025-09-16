using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce_Shop.Dtos
{
    public class SearchRawInputDto
    {
        [Required]
        public string Keyword { get; set; } = default!;

        [Range(0, double.MaxValue)]
        public decimal MinPrice { get; set; }
    }
}
