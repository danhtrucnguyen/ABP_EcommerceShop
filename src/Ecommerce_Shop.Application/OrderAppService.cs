using Ecommerce_Shop.Dtos;
using Ecommerce_Shop.Entities;
using Ecommerce_Shop.Events.Eto;
using Ecommerce_Shop.Permissions;
using Ecommerce_Shop.Services; 
using Ecommerce_Shop.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.EventBus.Local;


namespace Ecommerce_Shop
{
    public class OrderAppService : ApplicationService, IOrderAppService
    {
        private readonly IRepository<Order, Guid> _orderRepo;
        private readonly IRepository<OrderItem, Guid> _orderItemRepo;
        private readonly IRepository<Product, Guid> _productRepo;
        private readonly IRepository<Customer, Guid> _customerRepo;
        private readonly IRepository<Order, Guid> _orderRepository;

        private readonly ILocalEventBus _localEventBus;                 
        private readonly IDistributedEventBus _distributedEventBus;
        public OrderAppService(
            IRepository<Order, Guid> orderRepo,
            IRepository<OrderItem, Guid> orderItemRepo,
            IRepository<Product, Guid> productRepo,
            IRepository<Customer, Guid> customerRepo,
            IRepository<Order, Guid> orderRepository,
            ILocalEventBus localEventBus,                 
            IDistributedEventBus distributedEventBus)
        {
            _orderRepo = orderRepo;
            _orderItemRepo = orderItemRepo;
            _productRepo = productRepo;
            _customerRepo = customerRepo;
            _orderRepository = orderRepository;
            _localEventBus = localEventBus;               
            _distributedEventBus = distributedEventBus;   
        }

        public async Task<PagedResultDto<OrderDto>> GetListAsync(
            PagedAndSortedResultRequestDto input, Guid? customerId = null)
        {
            var q = (await _orderRepo.GetQueryableAsync())
                .Include(o => o.Customer)
                .Include(o => o.Items).ThenInclude(i => i.Product)
                .AsNoTracking();

            if (customerId.HasValue)
                q = q.Where(o => o.CustomerId == customerId.Value);

            var total = await q.CountAsync();

            var data = await q
                .OrderByDescending(x => x.CreationTime)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    CustomerId = o.CustomerId,
                    CustomerName = o.Customer != null ? o.Customer.Name : null,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status.ToString(),
                    CreationTime = o.CreationTime,
                    LastModificationTime = o.LastModificationTime,
                    Items = o.Items.Select(i => new OrderItemDto
                    {
                        Id = i.Id,
                        ProductId = i.ProductId,
                        ProductName = i.Product != null ? i.Product.Name : null,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        CreationTime = i.CreationTime,
                        LastModificationTime = i.LastModificationTime
                    }).ToList()
                })
                .ToListAsync();

            return new PagedResultDto<OrderDto>(total, data);
        }

        public async Task<OrderDto> GetAsync(Guid id)
        {
            var dto = await (await _orderRepo.GetQueryableAsync())
                .Include(o => o.Customer)
                .Include(o => o.Items).ThenInclude(i => i.Product)
                .AsNoTracking()
                .Where(o => o.Id == id)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    CustomerId = o.CustomerId,
                    CustomerName = o.Customer != null ? o.Customer.Name : null,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status.ToString(),
                    CreationTime = o.CreationTime,
                    LastModificationTime = o.LastModificationTime,
                    Items = o.Items.Select(i => new OrderItemDto
                    {
                        Id = i.Id,
                        ProductId = i.ProductId,
                        ProductName = i.Product != null ? i.Product.Name : null,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        CreationTime = i.CreationTime,
                        LastModificationTime = i.LastModificationTime
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (dto == null)
                throw new EntityNotFoundException(typeof(Order), id);

            return dto;
        }
        [Authorize(EcommerceShopPermissions.Orders.Create)]
        public async Task<OrderDto> CreateAsync(CreateOrderDto input)
        {
            var customer = await _customerRepo.FindAsync(input.CustomerId);
            if (customer == null)
                throw new EntityNotFoundException(typeof(Customer), input.CustomerId);

            var productIds = input.Items.Select(i => i.ProductId).Distinct().ToList();
            var products = await(await _productRepo.GetQueryableAsync())
                .Where(p => productIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id);

            var missing = productIds.Except(products.Keys).ToList();
            if (missing.Count > 0)
                throw new EntityNotFoundException(typeof(Product), missing[0]);

            var order = new Order(GuidGenerator.Create())
            {
                CustomerId = input.CustomerId,
                OrderDate = Clock.Now,
                Status = OrderStatus.Pending,
                TotalAmount = 0
            };

            foreach (var it in input.Items)
            {
                var price = it.UnitPrice > 0 ? it.UnitPrice : products[it.ProductId].Price;
                order.Items.Add(new OrderItem(GuidGenerator.Create())
                {
                    ProductId = it.ProductId,
                    Quantity = it.Quantity,
                    UnitPrice = price
                });
                order.TotalAmount += price * it.Quantity;
            }

            await _orderRepo.InsertAsync(order, true);
            await _distributedEventBus.PublishAsync(new OrderCreatedEto
            {
                OrderId = order.Id,
                CustomerId = order.CustomerId,
                TotalAmount = order.TotalAmount,
                CreatedAt = order.OrderDate,
                Items = order.Items.Select(i => new OrderItemEto
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            });
            return await MapReloadAsync(order.Id);
        }

        public Task<OrderDto> PayAsync(Guid id)
            => ChangeStatusAsync(id, OrderStatus.Pending, OrderStatus.Paid);

        public Task<OrderDto> ShipAsync(Guid id)
            => ChangeStatusAsync(id, OrderStatus.Paid, OrderStatus.Shipped);

        public Task<OrderDto> CompleteAsync(Guid id)
            => ChangeStatusAsync(id, OrderStatus.Shipped, OrderStatus.Completed);

        public async Task<OrderDto> CancelAsync(Guid id)
        {
            var order = await LoadOrderAsync(id);
            if (order.Status != OrderStatus.Pending && order.Status != OrderStatus.Paid)
                throw new BusinessException("InvalidStatusTransition")
                    .WithData("From", order.Status).WithData("To", OrderStatus.Canceled);

            order.Status = OrderStatus.Canceled;
            await _orderRepo.UpdateAsync(order, autoSave: true);
            return await MapReloadAsync(order.Id);
        }

        private async Task<Order> LoadOrderAsync(Guid id)
        {
            return await (await _orderRepo.GetQueryableAsync())
                .Include(o => o.Customer)
                .Include(o => o.Items).ThenInclude(i => i.Product)
                .FirstAsync(o => o.Id == id);
        }

        private async Task<OrderDto> ChangeStatusAsync(Guid id, OrderStatus expectedCurrent, OrderStatus next)
        {
            var order = await LoadOrderAsync(id);

            if (order.Status != expectedCurrent)
                throw new BusinessException("InvalidStatusTransition")
                    .WithData("From", order.Status)
                    .WithData("Expected", expectedCurrent)
                    .WithData("To", next);

            order.Status = next;
            await _orderRepo.UpdateAsync(order, autoSave: true);

            if (next == OrderStatus.Paid)
            {
                await _localEventBus.PublishAsync(
                    new OrderPaidEvent(order.Id, order.TotalAmount, Clock.Now)
                );
            }

            return await MapReloadAsync(order.Id);
        }

        private async Task<OrderDto> MapReloadAsync(Guid id)
        {
            var saved = await (await _orderRepo.GetQueryableAsync())
                .Include(o => o.Customer)
                .Include(o => o.Items).ThenInclude(i => i.Product)
                .FirstAsync(o => o.Id == id);

            return ObjectMapper.Map<Order, OrderDto>(saved);
        }

        public async Task<OrderFormLookupsDto> GetFormLookupsAsync()
        {
            var customers = await (await _customerRepo.GetQueryableAsync())
                .OrderBy(c => c.Name)
                .Select(c => new LookupDto<Guid> { Id = c.Id, DisplayName = c.Name })
                .ToListAsync();

            var products = await (await _productRepo.GetQueryableAsync())
                .OrderBy(p => p.Name)
                .Select(p => new LookupDto<Guid> { Id = p.Id, DisplayName = p.Name })
                .ToListAsync();

            return new OrderFormLookupsDto { Customers = customers, Products = products };
        }

        public async Task<OrderDto> GetOrderWithDetailsAsync(Guid id)
        {
            var queryable = await _orderRepository.WithDetailsAsync(x => x.Items); // ABP helper

            var order = await queryable
                .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                throw new EntityNotFoundException(typeof(Order), id);
            }

            return ObjectMapper.Map<Order, OrderDto>(order);
        }

        public async Task<OrderDto> GetOrderDetailsSpecAsync(Guid id)
        {
            var spec = new OrderWithDetailsSpecification(id);

            var q = (await _orderRepository.GetQueryableAsync())
                    .Where(spec.ToExpression());

            q = q.Include(o => o.Items)
                 .ThenInclude(i => i.Product);

            var order = await q.FirstOrDefaultAsync();
            if (order is null)
                throw new EntityNotFoundException(typeof(Order), id);

            return ObjectMapper.Map<Order, OrderDto>(order);
        }
    }
}
