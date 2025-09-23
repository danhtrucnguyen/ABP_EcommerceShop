using Ecommerce_Shop.Dtos;
using Ecommerce_Shop.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace Ecommerce_Shop.Controllers
{
    [ApiController]
    [Route("api/app/reviews")]
    public class ReviewController : AbpController
    {
        private readonly IReviewAppService _service;

        public ReviewController(ReviewAppService service )
        {
            _service = service;
        }

        [HttpGet]
        public Task<List<ReviewDto>> GetList() 
            => _service.GetListAsync();
        [HttpGet("{id}")]
        public Task<ReviewDto> GetAsync(Guid id)
            => _service.GetAsync(id);
        [HttpPost]
        public Task<ReviewDto> CreateAsync([FromBody]CreateUpdateReviewDto input)
            => _service.CreateAsync(input);
        [HttpPut("{id}")]
        public Task<ReviewDto> UpdateAsync(Guid id, [FromBody] CreateUpdateReviewDto input)
            => _service.UpdateAsync(id,input);
        [HttpDelete("{id}")]
        public Task DeleteAsync(Guid id)
            => _service.DeleteAsync(id);
    }
}
