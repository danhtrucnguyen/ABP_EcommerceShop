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
    [Route("api/categories")]
    public class CategoriesController : AbpController
    {
        private readonly ICategoryAppService _service;

        public CategoriesController(ICategoryAppService service)
        {
            _service = service;
        }

        // GET /api/categories?SkipCount=0&MaxResultCount=10&Sorting=Name
        [HttpGet]
        public Task<PagedResultDto<CategoryDto>> GetListAsync([FromQuery] PagedAndSortedResultRequestDto input)
            => _service.GetListAsync(input);

        // GET /api/categories/{id}
        [HttpGet("{id:guid}")]
        public Task<CategoryDto> GetAsync(Guid id) => _service.GetAsync(id);

        // POST /api/categories
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> CreateAsync([FromBody] CreateUpdateCategoryDto input)
        {
            var created = await _service.CreateAsync(input);
            return CreatedAtAction(nameof(GetAsync), new { id = created.Id }, created);
        }

        // PUT /api/categories/{id}
        [HttpPut("{id:guid}")]
        public Task<CategoryDto> UpdateAsync(Guid id, [FromBody] CreateUpdateCategoryDto input)
            => _service.UpdateAsync(id, input);

        // DELETE /api/categories/{id}
        [HttpDelete("{id:guid}")]
        public Task DeleteAsync(Guid id) => _service.DeleteAsync(id);
    }
}
