using Ecommerce_Shop.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Ecommerce_Shop.Services
{
    public interface IReviewAppService : IApplicationService
    {
        Task<ReviewDto> CreateAsync(CreateUpdateReviewDto input);
        Task<List<ReviewDto>> GetListAsync();
        Task<ReviewDto> GetAsync(Guid id);
        Task<ReviewDto> UpdateAsync(Guid id, CreateUpdateReviewDto input);
        Task DeleteAsync(Guid id);
    }
}
