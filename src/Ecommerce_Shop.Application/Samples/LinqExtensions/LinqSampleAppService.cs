using Ecommerce_Shop.Dtos;
using Ecommerce_Shop.Entities;
using Ecommerce_Shop.Samples;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Ecommerce_Shop
{
    public class LinqSampleAppService : ApplicationService
    {
        private readonly IRepository<Product, Guid> _productRepo;

        public LinqSampleAppService(IRepository<Product, Guid> productRepo)
        {
            _productRepo = productRepo;
        }

        /// GET /api/app/linq-sample/products
        /// /api/app/linq-sample/products?keyword=pro&minPrice=100000&sortBy=Price&asc=false&skip=0&take=10
        public async Task<PagedResultDto<ProductDto>> GetProductsAsync(
            string keyword = null, decimal? minPrice = null, decimal? maxPrice = null,
            string sortBy = "Name", bool asc = true, int skip = 0, int take = 10)
        {
            var queryable = await _productRepo.GetQueryableAsync();

            queryable = queryable
                .WhereIf(!string.IsNullOrWhiteSpace(keyword), p => EF.Functions.ILike(p.Name, $"%{keyword}%"))
                .WhereIf(minPrice.HasValue, p => p.Price >= minPrice!.Value)
                .WhereIf(maxPrice.HasValue, p => p.Price <= maxPrice!.Value);

            var ordered = queryable.OrderByProperty(string.IsNullOrWhiteSpace(sortBy) ? "Name" : sortBy, asc);
            var paged = ordered.PageBy(skip, take);



            var items = await paged.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
            }).ToListAsync();

            var totalCount = await queryable.CountAsync();

            return new PagedResultDto<ProductDto>(totalCount, items);
        }
    }
}
