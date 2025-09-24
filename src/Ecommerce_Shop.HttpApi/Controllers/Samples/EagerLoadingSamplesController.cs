using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ecommerce_Shop.Samples;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Ecommerce_Shop.HttpApi.Controllers.Samples
{
    [ApiController]
    [Route("api/app/eager-loading-samples")]
    public class EagerLoadingSamplesController : AbpController
    {
        private readonly EagerLoadingSamplesAppService _svc;

        public EagerLoadingSamplesController(EagerLoadingSamplesAppService svc)
        {
            _svc = svc;
        }

        // GET /api/app/eager-loading-samples/customer-orders?customerId=...&minQty=1&startDate=2025-09-01&endDate=2025-09-30
        [HttpGet("customer-orders")]
        public Task<List<EagerOrderDto>> GetCustomerOrdersEagerAsync(
            [FromQuery] Guid customerId,
            [FromQuery] int minQty = 1,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
            => _svc.GetCustomerOrdersEagerAsync(customerId, minQty, startDate, endDate);

        // GET /api/app/eager-loading-samples/product-details-eager?productId=...
        [HttpGet("product-details-eager")]
        public Task<EagerProductDetailsDto> GetProductDetailsEagerAsync([FromQuery] Guid productId)
            => _svc.GetProductDetailsEagerAsync(productId);

        // GET /api/app/eager-loading-samples/orders-with-customer?skip=0&take=10
        [HttpGet("orders-with-customer")]
        public Task<List<EagerOrderWithCustomerDto>> GetOrdersWithCustomerEagerAsync([FromQuery] int skip = 0, [FromQuery] int take = 10)
            => _svc.GetOrdersWithCustomerEagerAsync(skip, take);
    }
}
