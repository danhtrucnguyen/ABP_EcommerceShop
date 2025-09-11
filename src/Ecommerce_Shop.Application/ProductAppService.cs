using Ecommerce_Shop.Caching;
using Ecommerce_Shop.Dtos;
using Ecommerce_Shop.Entities;
using Ecommerce_Shop.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed; // <-- cần cho DistributedCacheEntryOptions
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace Ecommerce_Shop
{
    public class ProductAppService :
        CrudAppService<
            Product,
            ProductDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateUpdateProductDto,
            CreateUpdateProductDto>,
        IProductAppService
    {
        private readonly IRepository<Category, Guid> _categoryRepository;
        private readonly IRepository<Product, Guid> _productRepo;     // dùng cho các truy vấn LINQ
        private readonly IRepository<OrderItem, Guid> _orderItemRepo;
        private readonly IRepository<Order, Guid> _orderRepo;
        private readonly IDataFilter _dataFilter;
        private readonly IDistributedCache<ProductCacheItem, Guid> _productCache;

        public ProductAppService(
            IRepository<Product, Guid> productRepository,                
            IRepository<Category, Guid> categoryRepository,
            IRepository<Product, Guid> productRepo,                      
            IRepository<OrderItem, Guid> orderItemRepo,
            IRepository<Order, Guid> orderRepo,
            IDataFilter dataFilter,
            IDistributedCache<ProductCacheItem, Guid> productCache
        ) : base(productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepo = productRepo;         
            _orderItemRepo = orderItemRepo;
            _orderRepo = orderRepo;
            _dataFilter = dataFilter;
            _productCache = productCache;
        }

        //CREATE invalidate cache
        public override async Task<ProductDto> CreateAsync(CreateUpdateProductDto input)
        {
            if (input.CategoryId.HasValue)
            {
                var exists = await _categoryRepository.AnyAsync(x => x.Id == input.CategoryId.Value);
                if (!exists)
                {
                    throw new BusinessException("CategoryNotFound")
                        .WithData("CategoryId", input.CategoryId);
                }
            }

            var entity = ObjectMapper.Map<CreateUpdateProductDto, Product>(input);
            await Repository.InsertAsync(entity, autoSave: true);

            // clear cache
            await _productCache.RemoveAsync(entity.Id);

            return ObjectMapper.Map<Product, ProductDto>(entity);
        }

        //UPDATE invalidate cache
        public override async Task<ProductDto> UpdateAsync(Guid id, CreateUpdateProductDto input)
        {
            if (input.CategoryId.HasValue)
            {
                var exists = await _categoryRepository.AnyAsync(x => x.Id == input.CategoryId.Value);
                if (!exists)
                {
                    throw new BusinessException("CategoryNotFound")
                        .WithData("CategoryId", input.CategoryId);
                }
            }

            var entity = await Repository.GetAsync(id);
            ObjectMapper.Map(input, entity);
            await Repository.UpdateAsync(entity, autoSave: true);

            await _productCache.RemoveAsync(id);

            return ObjectMapper.Map<Product, ProductDto>(entity);
        }

        //DELETE (invalidate cache)
        [UnitOfWork]
        public override async Task DeleteAsync(Guid id)
        {
            await base.DeleteAsync(id);
            await _productCache.RemoveAsync(id);
        }

        [UnitOfWork]
        public virtual async Task ChangePriceAsync(Guid id, ChangeProductPriceDto input)
        {
            var product = await Repository.GetAsync(id);
            product.ChangePrice(input.NewPrice);     
            await Repository.UpdateAsync(product, autoSave: true);

            await _productCache.RemoveAsync(id);     // xoá cache sau khi đổi giá
        }

        public override async Task<ProductDto> GetAsync(Guid id)
        {
            var cacheItem = await _productCache.GetOrAddAsync(
                id,
                async () =>
                {
                    var entity = await Repository.GetAsync(id);
                    return new ProductCacheItem
                    {
                        Id = entity.Id,
                        Name = entity.Name,
                        Price = entity.Price,
                        Description = entity.Description,
                        CategoryId = entity.CategoryId
                    };
                },
                () => new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                }
            );

            return new ProductDto
            {
                Id = cacheItem.Id,
                Name = cacheItem.Name,
                Price = cacheItem.Price,
                Description = cacheItem.Description,
                CategoryId = cacheItem.CategoryId
            };
        }

        // ========= LINQ helpers =========
        public async Task<List<ProductDto>> GetNeverOrderedAsync()
        {
            var productsQ = await _productRepo.GetQueryableAsync();
            var orderItemsQ = await _orderItemRepo.GetQueryableAsync();

            var query =
                from p in productsQ
                where !orderItemsQ.Any(oi => oi.ProductId == p.Id)
                select p;

            var entities = await AsyncExecuter.ToListAsync(query);
            return ObjectMapper.Map<List<Product>, List<ProductDto>>(entities);
        }

        public async Task<List<ProductDto>> GetPurchasedByCustomerAsync(Guid customerId, DateTime fromDate, DateTime toDate)
        {
            var productsQ = await _productRepo.GetQueryableAsync();
            var orderItemsQ = await _orderItemRepo.GetQueryableAsync();
            var ordersQ = await _orderRepo.GetQueryableAsync();

            var query =
                from p in productsQ
                where orderItemsQ.Any(oi =>
                      oi.ProductId == p.Id &&
                      ordersQ.Any(o =>
                           o.Id == oi.OrderId &&
                           o.CustomerId == customerId &&
                           o.CreationTime >= fromDate &&
                           o.CreationTime < toDate))
                select p;

            var entities = await AsyncExecuter.ToListAsync(query);
            return ObjectMapper.Map<List<Product>, List<ProductDto>>(entities);
        }

        public async Task<List<ProductDto>> GetAllIncludingDeletedAsync()
        {
            using (_dataFilter.Disable<ISoftDelete>())
            {
                var q = await Repository.GetQueryableAsync();
                var list = await AsyncExecuter.ToListAsync(q);
                return ObjectMapper.Map<List<Product>, List<ProductDto>>(list);
            }
        }

        public async Task<List<ProductDto>> GetDeletedAsync()
        {
            using (_dataFilter.Disable<ISoftDelete>())
            {
                var q = await Repository.GetQueryableAsync();
                var onlyDeleted = q.Where(p => p.IsDeleted);
                var list = await AsyncExecuter.ToListAsync(onlyDeleted);
                return ObjectMapper.Map<List<Product>, List<ProductDto>>(list);
            }
        }
    }
}
