using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_Shop.Entities;
using Ecommerce_Shop.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Application.Services;

namespace Ecommerce_Shop.Samples
{
    public class EagerLoadingSamplesAppService : ApplicationService
    {
        private readonly Ecommerce_ShopDbContext _db;

        public EagerLoadingSamplesAppService(Ecommerce_ShopDbContext db)
        {
            _db = db;
        }

        public async Task<List<EagerOrderDto>> GetCustomerOrdersEagerAsync(
     Guid customerId,
     int minQty = 1,
     DateTime? startDate = null,
     DateTime? endDate = null)
        {
            var baseQuery = _db.Set<Order>()
                .AsNoTracking()
                .Where(o => o.CustomerId == customerId);

            if (startDate.HasValue) baseQuery = baseQuery.Where(o => o.OrderDate >= startDate.Value);
            if (endDate.HasValue) baseQuery = baseQuery.Where(o => o.OrderDate <= endDate.Value);

            var orders = await baseQuery
                .OrderByDescending(o => o.OrderDate)
                .Select(o => new
                {
                    o.Id,
                    o.OrderDate,
                    o.TotalAmount
                })
                .ToListAsync();

            if (orders.Count == 0)
                return new List<EagerOrderDto>();

            var orderIds = orders.Select(x => x.Id).ToList();

            var orderItems = await _db.Set<OrderItem>()
                .AsNoTracking()
                .Where(oi => orderIds.Contains(oi.OrderId) && oi.Quantity >= minQty)
                .Select(oi => new
                {
                    oi.OrderId,
                    oi.ProductId,
                    oi.Quantity,
                    oi.UnitPrice
                })
                .ToListAsync();

            var productIds = orderItems.Select(oi => oi.ProductId).Distinct().ToList();

            var products = await _db.Set<Product>()
                .AsNoTracking()
                .Where(p => productIds.Contains(p.Id))
                .Select(p => new { p.Id, p.Name })
                .ToListAsync();

            var productById = products.ToDictionary(p => p.Id, p => p.Name);

            var itemsByOrder = orderItems
                .GroupBy(oi => oi.OrderId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(oi => new EagerOrderItemDto
                    {
                        ProductId = oi.ProductId,
                        ProductName = productById.TryGetValue(oi.ProductId, out var name) ? name : string.Empty,
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice
                    }).ToList()
                );

            var result = orders.Select(o => new EagerOrderDto
            {
                OrderId = o.Id,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Items = itemsByOrder.TryGetValue(o.Id, out var items) ? items : new List<EagerOrderItemDto>()
            })
            .ToList();

            return result;
        }

        public async Task<EagerProductDetailsDto> GetProductDetailsEagerAsync(Guid productId)
        {
            var productBasic = await _db.Set<Product>()
                .AsNoTracking()
                .Where(p => p.Id == productId)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Price,
                    p.CategoryId
                })
                .FirstOrDefaultAsync();

            if (productBasic == null)
                return null!;

            string? categoryName = null;
            if (productBasic.CategoryId.HasValue)
            {
                categoryName = await _db.Set<Category>()
                    .AsNoTracking()
                    .Where(c => c.Id == productBasic.CategoryId.Value)
                    .Select(c => c.Name)
                    .FirstOrDefaultAsync();
            }

            var reviewsCount = await _db.Set<Review>()
                .AsNoTracking()
                .Where(r => r.ProductId == productBasic.Id)
                .CountAsync();

            return new EagerProductDetailsDto
            {
                ProductId = productBasic.Id,
                Name = productBasic.Name,
                Price = productBasic.Price,
                CategoryId = productBasic.CategoryId,
                CategoryName = categoryName,
                ReviewsCount = reviewsCount
            };
        }

        public async Task<List<EagerOrderWithCustomerDto>> GetOrdersWithCustomerEagerAsync(int skip = 0, int take = 10)
        {
            var data = await _db.Set<Order>()
                .AsNoTracking()
                .Include(o => o.Customer)
                .OrderByDescending(o => o.OrderDate)
                .Skip(skip)
                .Take(take)
                .Select(o => new EagerOrderWithCustomerDto
                {
                    OrderId = o.Id,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    CustomerId = o.CustomerId,
                    CustomerName = o.Customer != null ? o.Customer.Name : string.Empty,
                    CustomerEmail = o.Customer != null ? o.Customer.Email : null
                })
                .ToListAsync();

            return data;
        }
    }


    public class EagerOrderItemDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = "";
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class EagerOrderDto
    {
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<EagerOrderItemDto> Items { get; set; } = new();
    }

    public class EagerProductDetailsDto
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public Guid? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int ReviewsCount { get; set; }
    }

    public class EagerOrderWithCustomerDto
    {
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; } = "";
        public string? CustomerEmail { get; set; }
    }
}
