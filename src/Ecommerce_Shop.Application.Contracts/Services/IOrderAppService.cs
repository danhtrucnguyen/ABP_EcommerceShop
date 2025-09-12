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
    public interface IOrderAppService : IApplicationService
    {
        Task<PagedResultDto<OrderDto>> GetListAsync(PagedAndSortedResultRequestDto input, Guid? customerId = null);
        Task<OrderDto> GetAsync(Guid id);

        Task<OrderDto> CreateAsync(CreateOrderDto input);

        Task<OrderDto> PayAsync(Guid id);
        Task<OrderDto> ShipAsync(Guid id);
        Task<OrderDto> CompleteAsync(Guid id);
        Task<OrderDto> CancelAsync(Guid id);

        Task<OrderFormLookupsDto> GetFormLookupsAsync();

        Task<OrderDto> GetOrderWithDetailsAsync(Guid id);
    }
}
