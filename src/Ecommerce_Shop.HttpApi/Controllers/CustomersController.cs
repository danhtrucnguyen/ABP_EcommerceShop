using Ecommerce_Shop.Dtos;
using Ecommerce_Shop.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Ecommerce_Shop.Controllers
{
    [Route("api/customers")]
    public class CustomersController : AbpController
    {
        private readonly ICustomerAppService _service;

        public CustomersController(ICustomerAppService service)
        {
            _service = service;
        }

        //https://localhost:44356/api/customers
        [HttpGet]
        public Task<PagedResultDto<CustomerDto>> GetListAsync([FromQuery] PagedAndSortedResultRequestDto input)
            => _service.GetListAsync(input);

        // https://localhost:44356/api/customers/{id}
        [HttpGet("{id:guid}")]
        public Task<CustomerDto> GetAsync(Guid id) => _service.GetAsync(id);

        //https://localhost:44356/api/customers
        //{ "name": "Hoa", "email": "abc@gmail.com", "phone": "0925668548", "address": "Hanoi" }
        [HttpPost]
        public async Task<ActionResult<CustomerDto>> CreateAsync([FromBody] CreateUpdateCustomerDto input)
        {
            var created = await _service.CreateAsync(input);
            return CreatedAtAction(nameof(GetAsync), new { id = created.Id }, created);
        }

        // https://localhost:44356/api/customers/{id}
        [HttpPut("{id:guid}")]
        public Task<CustomerDto> UpdateAsync(Guid id, [FromBody] CreateUpdateCustomerDto input)
            => _service.UpdateAsync(id, input);

        // https://localhost:44356/api/customers/{id}
        [HttpDelete("{id:guid}")]
        public Task DeleteAsync(Guid id) => _service.DeleteAsync(id);
    }
}
