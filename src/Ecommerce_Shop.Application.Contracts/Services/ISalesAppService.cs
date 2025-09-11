using Ecommerce_Shop.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Ecommerce_Shop.Services
{
    public interface ISalesAppService : IApplicationService
    {
        Task<OrderDto> PlaceOrderAndAnnotateProductsAsync(CreateOrderDto input);

    }
}
