//using Microsoft.AspNetCore.Mvc;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
namespace Ecommerce_Shop.Samples;
public interface ICustomerExplicitSamplesAppService : IApplicationService
{
    Task<CustomerOrdersExplicitResultDto> GetCustomerOrdersExplicitAsync(
        Guid customerId,
        int minQty,
        DateTime? startDate,
        DateTime? endDate);
}