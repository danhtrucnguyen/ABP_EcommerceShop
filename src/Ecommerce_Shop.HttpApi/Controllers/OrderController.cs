using Ecommerce_Shop.Dtos;
using Ecommerce_Shop.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Ecommerce_Shop.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderAppService _service;
        public OrdersController(IOrderAppService service) => _service = service;

        [HttpGet]
        public Task<PagedResultDto<OrderDto>> GetListAsync(
            [FromQuery] PagedAndSortedResultRequestDto input,
            [FromQuery] Guid? customerId) => _service.GetListAsync(input, customerId);

        [HttpGet("{id:guid}")]
        public Task<OrderDto> GetAsync(Guid id) => _service.GetAsync(id);

        //https://localhost:44356/api/orders
        /*
         {
          "customerId": "3a1c39c5-9923-33eb-e191-c3a9a68f16eb",
          "items": [
            {
              "productId": "3a1c2a67-b36d-f01a-5f40-65b4a2c4875e",
              "quantity": 2
      
            }
          ]
        }
         */
        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateAsync([FromBody] CreateOrderDto input)
        {
            var created = await _service.CreateAsync(input);
            return CreatedAtAction(nameof(GetAsync), new { id = created.Id }, created);
        }

        [HttpPost("{id:guid}/pay")] public Task<OrderDto> PayAsync(Guid id) => _service.PayAsync(id);// thanh toan
        [HttpPost("{id:guid}/ship")] public Task<OrderDto> ShipAsync(Guid id) => _service.ShipAsync(id);//giao hang
        [HttpPost("{id:guid}/complete")] public Task<OrderDto> CompleteAsync(Guid id) => _service.CompleteAsync(id);//nhan hang
        [HttpPost("{id:guid}/cancel")] public Task<OrderDto> CancelAsync(Guid id) => _service.CancelAsync(id);//huy don

        [HttpGet("{id}/details")]
        public Task<OrderDto> GetOrderDetails(Guid id) => _service.GetOrderWithDetailsAsync(id);
    }
}

