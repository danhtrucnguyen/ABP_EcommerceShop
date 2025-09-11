using Ecommerce_Shop.Dtos;
using Ecommerce_Shop.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce_Shop.Controllers
{
    [ApiController]
    [Route("api/sales")]
    [Produces(MediaTypeNames.Application.Json)]
    public class SalesController : ControllerBase
    {
        private readonly ISalesAppService _service;
        public SalesController(ISalesAppService service) => _service = service;

        // POST https://localhost:44356/api/sales/place
        [HttpPost("place")]
        public Task<OrderDto> Place([FromBody] CreateOrderDto input)
            => _service.PlaceOrderAndAnnotateProductsAsync(input);

    }

}
