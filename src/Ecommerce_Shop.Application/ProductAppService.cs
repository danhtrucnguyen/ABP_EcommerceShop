using Ecommerce_Shop.Dtos;
using Ecommerce_Shop.Entities;
using Ecommerce_Shop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Ecommerce_Shop
{
    public class ProductAppService :
        CrudAppService<
            Product,                    // Entity
            ProductDto,                 // DTO trả về
            Guid,                       // Kiểu khóa chính
            PagedAndSortedResultRequestDto, // Input cho GET list
            CreateUpdateProductDto,     // DTO tạo
            CreateUpdateProductDto>,    // DTO cập nhật
        IProductAppService
    {
        private readonly IRepository<Category, Guid> _categoryRepository;

        public ProductAppService(
            IRepository<Product, Guid> productRepository,
            IRepository<Category, Guid> categoryRepository
        ) : base(productRepository)
        {
            _categoryRepository = categoryRepository;
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
    }
}