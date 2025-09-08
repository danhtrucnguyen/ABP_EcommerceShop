using Ecommerce_Shop.Dtos;
using Ecommerce_Shop.Entities;
using Ecommerce_Shop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Ecommerce_Shop
{
    public class CategoryAppService :
    CrudAppService<
        Category,                    // entity
        CategoryDto,                 // dto
        Guid,                        // key
        PagedAndSortedResultRequestDto,
        CreateUpdateCategoryDto,
        CreateUpdateCategoryDto>,
    ICategoryAppService
    {
        public CategoryAppService(IRepository<Category, Guid> repository)
            : base(repository)
        {
        }
    }
}
