using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Application.Services;
using Volo.Abp.Uow;
using Ecommerce_Shop.EntityFrameworkCore;
using Ecommerce_Shop.Entities;
using Ecommerce_Shop.Samples; // để dùng DTO

namespace Ecommerce_Shop.Samples
{
    public class CustomerExplicitSamplesAppService
        : ApplicationService, ICustomerExplicitSamplesAppService
    {
        private readonly Ecommerce_ShopDbContext _db;
        public CustomerExplicitSamplesAppService(Ecommerce_ShopDbContext db) => _db = db;

        [UnitOfWork]
        public async Task<CustomerOrdersExplicitResultDto> GetCustomerOrdersExplicitAsync(
            Guid customerId, int minQty, DateTime? startDate, DateTime? endDate)
        {
            var customer = await _db.Customers.FirstOrDefaultAsync(c => c.Id == customerId);
            if (customer == null) throw new EntityNotFoundException(typeof(Customer), customerId);

            var ordersQuery = _db.Orders.Where(o => o.CustomerId == customerId);
            if (startDate.HasValue) ordersQuery = ordersQuery.Where(o => o.CreationTime >= startDate.Value);
            if (endDate.HasValue)   ordersQuery = ordersQuery.Where(o => o.CreationTime <  endDate.Value);

            var orders = await ordersQuery.ToListAsync(); // tracking

            foreach (var order in orders)
            {
                var itemsQ = _db.Entry(order).Collection(o => o.Items).Query();
                if (minQty > 1) itemsQ = itemsQ.Where(i => i.Quantity >= minQty);
                await itemsQ.LoadAsync();

                foreach (var it in order.Items)
                    await _db.Entry(it).Reference(ii => ii.Product).LoadAsync();
            }

            return new CustomerOrdersExplicitResultDto
            {
                CustomerId = customer.Id,
                CustomerName = customer.Name,
                TotalOrders = orders.Count,
                TotalItems  = orders.Sum(o => o.Items.Count),
                Orders = orders.Select(o => new OrderBriefDto
                {
                    OrderId   = o.Id,
                    ItemCount = o.Items.Count,
                    Items     = o.Items.Select(i => new ItemBriefDto
                    {
                        OrderItemId = i.Id,
                        ProductId   = i.ProductId,
                        ProductName = i.Product?.Name,
                        Quantity    = i.Quantity
                    }).ToList()
                }).ToList()
            };
        }
    }
}
