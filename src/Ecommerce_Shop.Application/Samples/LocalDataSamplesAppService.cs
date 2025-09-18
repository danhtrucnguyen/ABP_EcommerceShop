using Ecommerce_Shop.Entities;
using Ecommerce_Shop.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Guids;
using Volo.Abp.Uow;
using Microsoft.EntityFrameworkCore;
using Ecommerce_Shop.Dtos;

namespace Ecommerce_Shop.Samples
{
    // /api/app/local-data-samples/demo?orderId=...&productId=...
    public class LocalDataSamplesAppService : ApplicationService
    {
        private readonly Ecommerce_ShopDbContext _db;

        public LocalDataSamplesAppService(Ecommerce_ShopDbContext db)
        {
            _db = db;
        }

       
        [UnitOfWork]
        public async Task<LocalDataSnapshotDto> DemoAsync(Guid orderId, Guid productId)
        {
            //Lấy Order + Items để DbContext bắt đầu tracking
            var order = await _db.Orders
                                 .Include(o => o.Items)
                                 .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                throw new EntityNotFoundException(typeof(Order), orderId);

            //Thêm 1 item mới (CHƯA lưu DB)
            var newItem = new OrderItem(GuidGenerator.Create())
            {
                OrderId = orderId,
                ProductId = productId,
                Quantity = 1,
                UnitPrice = 0m
            };
            order.Items.Add(newItem);

            //Đọc Local cache
            var localOrders = _db.Set<Order>().Local.ToList();
            var localItems = _db.Set<OrderItem>().Local.ToList();
            var localProds = _db.Set<Product>().Local.ToList();

            return new LocalDataSnapshotDto
            {
                TrackedOrders = localOrders.Select(o => o.Id).ToList(),
                TrackedItems = localItems.Select(i => new TrackedItemDto
                {
                    Id = i.Id,
                    OrderId = i.OrderId,
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                }).ToList(),
                TrackedProducts = localProds.Select(p => p.Id).ToList(),
                PendingChangesCount = _db.ChangeTracker.Entries().Count()
            };
        }
    }
}
