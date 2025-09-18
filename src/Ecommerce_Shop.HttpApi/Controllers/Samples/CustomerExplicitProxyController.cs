using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Ecommerce_Shop.Samples;

namespace Ecommerce_Shop.HttpApi.Controllers.Samples
{
    [ApiController]
    [Route("api/app/explicit-loading")]
    public class CustomerExplicitProxyController : AbpController
    {
        private readonly ICustomerExplicitSamplesAppService _svc;
        public CustomerExplicitProxyController(ICustomerExplicitSamplesAppService svc) => _svc = svc;

        [HttpGet("customer-orders")]
        public Task<CustomerOrdersExplicitResultDto> Get(
            Guid customerId, int minQty = 1, DateTime? startDate = null, DateTime? endDate = null)
            => _svc.GetCustomerOrdersExplicitAsync(customerId, minQty, startDate, endDate);
    }
}
