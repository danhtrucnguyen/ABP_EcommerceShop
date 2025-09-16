using Ecommerce_Shop.Dtos;
using Ecommerce_Shop.Entities;
using Ecommerce_Shop.EntityFrameworkCore;  
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Ecommerce_Shop.Samples
{
    public class EfCoreQuerySamplesAppService : ApplicationService
    {
        private readonly Ecommerce_ShopDbContext _dbContext;

        public EfCoreQuerySamplesAppService(Ecommerce_ShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // POST /api/app/ef-core-query-samples/search-raw
        public async Task<List<ProductDto>> SearchRawAsync(SearchRawInputDto input)
        {
            var sqlQuery = _dbContext.Set<Product>().FromSqlInterpolated($@"
        SELECT * FROM ""AppProducts""
        WHERE ""Price"" > {input.MinPrice}
          AND ""Name"" ILIKE {'%' + input.Keyword + '%'}");

            return await sqlQuery.AsNoTracking()
                .Select(p => new ProductDto { Id = p.Id, Name = p.Name, Price = p.Price })
                .ToListAsync();
        }
    }
}
