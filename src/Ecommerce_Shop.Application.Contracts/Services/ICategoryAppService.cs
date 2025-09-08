using Ecommerce_Shop.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Ecommerce_Shop.Services
{
    public interface ICategoryAppService :
     ICrudAppService<
         CategoryDto,                    // DTO trả về
         Guid,                           // khóa chính
         PagedAndSortedResultRequestDto, // paging/sorting
         CreateUpdateCategoryDto,        // input create
         CreateUpdateCategoryDto         // input update
     >
    { }
}
