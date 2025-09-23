using Ecommerce_Shop.Dtos;
using Ecommerce_Shop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;


namespace Ecommerce_Shop.Services
{
    public class ReviewAppService : ApplicationService, IReviewAppService
    {
        private readonly IRepository<Review, Guid> _reviewRepo;

        public ReviewAppService(IRepository<Review, Guid> reviewRepo)
        {
            _reviewRepo = reviewRepo;
        }

        public async Task<ReviewDto> CreateAsync(CreateUpdateReviewDto input)
        {
            var review = new Review
            {
                
                ProductId = input.ProductId,
                CustomerId = input.CustomerId,
                Rating = input.Rating,
                Title = input.Title,
                Content = input.Content,
                IsApproved = false
            };

            EntityHelper.TrySetId(review, () => GuidGenerator.Create());

            //await _reviewRepo.InsertAsync(review);
            //return ObjectMapper.Map<Review, ReviewDto>(review);
            await _reviewRepo.InsertAsync(review, autoSave: true);
            return ObjectMapper.Map<Review, ReviewDto>(review);
        }

        public async Task<List<ReviewDto>> GetListAsync()
        {
            var list = await _reviewRepo.GetListAsync();
            return list.Select(x => ObjectMapper.Map<Review, ReviewDto>(x)).ToList();
        }

        public async Task<ReviewDto> GetAsync(Guid id)
        {
            var review = await _reviewRepo.GetAsync(id);
            return ObjectMapper.Map<Review, ReviewDto>(review);
        }

        public async Task<ReviewDto> UpdateAsync(Guid id, CreateUpdateReviewDto input)
        {
            var review = await _reviewRepo.GetAsync(id);
            review.Rating = input.Rating;
            review.Title = input.Title;
            review.Content = input.Content;
            await _reviewRepo.UpdateAsync(review, autoSave: true);
            return ObjectMapper.Map<Review, ReviewDto>(review);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _reviewRepo.DeleteAsync(id);
        }
    }
}
