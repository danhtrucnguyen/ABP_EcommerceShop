using Ecommerce_Shop.Dtos;
using Ecommerce_Shop.Entities;
using Ecommerce_Shop.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
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
        private readonly IRepository<Product, Guid> _productRepo;
        private readonly IRepository<OrderItem, Guid> _orderItemRepo;
        private readonly IRepository<Order, Guid> _orderRepo;
        private readonly IDataFilter _dataFilter;
        private readonly IRepository<Product, Guid> _productRepository;

        public ProductAppService(
            IRepository<Product, Guid> productRepository,
            IRepository<Category, Guid> categoryRepository,
            IRepository<Product, Guid> productRepo,
            IRepository<OrderItem, Guid> orderItemRepo,
            IRepository<Order, Guid> orderRepo,
            IDataFilter dataFilter
        ) : base(productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepo = productRepo;
            _orderItemRepo = orderItemRepo;
            _orderRepo = orderRepo;
            _dataFilter = dataFilter;
        }

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

            return ObjectMapper.Map<Product, ProductDto>(entity);
        }

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

            return ObjectMapper.Map<Product, ProductDto>(entity);
        }
        
        //LINQ
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
                var onlyDeleted = q.Where(p => p.IsDeleted);
                var list = await AsyncExecuter.ToListAsync(q);
                return ObjectMapper.Map<List<Product>, List<ProductDto>>(list);
            }
        }
        public async Task<List<ProductDto>> GetDeletedAsync()
        {
            using (_dataFilter.Disable<ISoftDelete>())
            {
                var q = await Repository.GetQueryableAsync();
                var onlyDeleted = q.Where(p => p.IsDeleted); // Product phải implement ISoftDelete
                var list = await AsyncExecuter.ToListAsync(onlyDeleted);
                return ObjectMapper.Map<List<Product>, List<ProductDto>>(list);
            }
        }

        [UnitOfWork]
        public virtual async Task ChangePriceAsync(Guid id, ChangeProductPriceDto input)
        {
            // dùng Repository của CrudAppService
            var product = await Repository.GetAsync(id);

            product.ChangePrice(input.NewPrice);   

            await Repository.UpdateAsync(product, autoSave: true);
        }
    }

}
