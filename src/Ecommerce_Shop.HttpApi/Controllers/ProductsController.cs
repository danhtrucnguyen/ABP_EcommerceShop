using Ecommerce_Shop.Dtos;
using Ecommerce_Shop.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;



namespace Ecommerce_Shop.Controllers
{
    [ApiController]
    [Route("api/products")]
    [Produces(MediaTypeNames.Application.Json)]
    public class ProductsController : ControllerBase
    {
        private readonly IProductAppService _service;


        public ProductsController(IProductAppService service) => _service = service;

        [HttpGet]
        public Task<PagedResultDto<ProductDto>> GetListAsync([FromQuery] PagedAndSortedResultRequestDto input)
            => _service.GetListAsync(input);

        [HttpGet("{id:guid}")]
        public Task<ProductDto> GetAsync(Guid id) => _service.GetAsync(id);

        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateAsync([FromBody] CreateUpdateProductDto input)
        {
            var created = await _service.CreateAsync(input);
            return CreatedAtAction(nameof(GetAsync), new { id = created.Id }, created);
        }

        [HttpPut("{id:guid}")]
        public Task<ProductDto> UpdateAsync(Guid id, [FromBody] CreateUpdateProductDto input)
            => _service.UpdateAsync(id, input);

        [HttpDelete("{id:guid}")]
        public Task DeleteAsync(Guid id) => _service.DeleteAsync(id);

        [HttpGet("never-ordered")]
        public async Task<ActionResult<List<ProductDto>>> GetNeverOrderedAsync()
        {
            var result = await _service.GetNeverOrderedAsync();
            return Ok(result);
        }

        [HttpGet("purchased-by-customer")]
        public async Task<ActionResult<List<ProductDto>>> GetPurchasedByCustomerAsync(
            [FromQuery] Guid customerId,
            [FromQuery] DateTime fromDate,
            [FromQuery] DateTime? toDate)
        {
            if (customerId == Guid.Empty)
                return BadRequest("customerId is required");

            var until = toDate ?? DateTime.MaxValue;
            var result = await _service.GetPurchasedByCustomerAsync(customerId, fromDate, until);
            return Ok(result);
        }

        [HttpGet("all-including-deleted")]
        public async Task<ActionResult<List<ProductDto>>> GetAllIncludingDeletedAsync()
        {
            var result = await _service.GetAllIncludingDeletedAsync();
            return Ok(result);
        }

        [HttpGet("deleted")]
        public async Task<ActionResult<List<ProductDto>>> GetDeletedAsync()
        {
            var result = await _service.GetDeletedAsync();
            return Ok(result);
        }

    }
}
