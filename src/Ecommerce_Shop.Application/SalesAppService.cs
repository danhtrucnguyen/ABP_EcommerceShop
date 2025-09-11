using Ecommerce_Shop.Dtos;
using Ecommerce_Shop.Entities;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;
using System;
using System.Threading.Tasks;

namespace Ecommerce_Shop.Services
{
    public class SalesAppService : ApplicationService, ISalesAppService
    {
        private readonly IOrderAppService _orderApp;
        private readonly IRepository<Product, Guid> _productRepo;

        public SalesAppService(
            IOrderAppService orderApp,
            IRepository<Product, Guid> productRepo)
        {
            _orderApp = orderApp;
            _productRepo = productRepo;
        }

        [UnitOfWork]
        public async Task<OrderDto> PlaceOrderAndAnnotateProductsAsync(CreateOrderDto input)
        {
            var order = await _orderApp.CreateAsync(input);

            foreach (var reqItem in input.Items)
            {
                var p = await _productRepo.FindAsync(reqItem.ProductId);
                if (p == null)
                {

                    throw new BusinessException("ProductNotFound")
                        .WithData("ProductId", reqItem.ProductId);
                }

                var newDesc = string.IsNullOrWhiteSpace(p.Description)
                    ? $"(ordered qty {reqItem.Quantity} at {reqItem.UnitPrice})"
                    : $"{p.Description} | ordered qty {reqItem.Quantity} at {reqItem.UnitPrice}";

                p.Update(p.Name, p.Price, newDesc, p.CategoryId);
                await _productRepo.UpdateAsync(p, autoSave: true);
            }

            return order;
        }
    }
}
