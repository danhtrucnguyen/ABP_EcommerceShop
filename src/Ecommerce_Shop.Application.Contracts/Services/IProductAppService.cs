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
    public interface IProductAppService :
    ICrudAppService<
        ProductDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateProductDto,
        CreateUpdateProductDto>
    {
        Task<List<ProductDto>> GetNeverOrderedAsync();

        Task<List<ProductDto>> GetPurchasedByCustomerAsync(
            Guid customerId,
            DateTime fromDate,
            DateTime toDate);
    }
}
