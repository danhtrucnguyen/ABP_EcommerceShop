using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce_Shop.Dtos
{
    public class ChangeProductPriceDto
    {
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335",
             ErrorMessage = "Giá sản phẩm phải lớn hơn 0")]
        public decimal NewPrice { get; set; }
    }

}


