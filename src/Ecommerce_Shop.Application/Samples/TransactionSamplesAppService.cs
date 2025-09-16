using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ecommerce_Shop.Entities;
using Ecommerce_Shop.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Uow;

namespace Ecommerce_Shop
{
    /// <summary>
    /// Ví dụ Transaction & Connection Resiliency cho PostgreSQL trong ABP.
    /// </summary>
    public class TransactionSamplesAppService : ApplicationService
    {
        private readonly IRepository<Product, Guid> _productRepo;
        private readonly IRepository<Order, Guid> _orderRepo;
        private readonly IRepository<OrderItem, Guid> _orderItemRepo;
        private readonly IUnitOfWorkManager _uowManager;
        private readonly IDbContextProvider<Ecommerce_ShopDbContext> _dbContextProvider;

        public TransactionSamplesAppService(
            IRepository<Product, Guid> productRepo,
            IRepository<Order, Guid> orderRepo,
            IRepository<OrderItem, Guid> orderItemRepo,
            IUnitOfWorkManager uowManager,
            IDbContextProvider<Ecommerce_ShopDbContext> dbContextProvider)
        {
            _productRepo = productRepo;
            _orderRepo = orderRepo;
            _orderItemRepo = orderItemRepo;
            _uowManager = uowManager;
            _dbContextProvider = dbContextProvider;
        }

        public class PlaceOrderItemInput
        {
            public Guid ProductId { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
        }

        public class PlaceOrderInput
        {
            public Guid CustomerId { get; set; }
            public List<PlaceOrderItemInput> Items { get; set; } = new();
        }

        [UnitOfWork(IsDisabled = true)]
        public async Task<Guid> PlaceOrderResilientAsync(PlaceOrderInput input)
        {
            if (input.Items == null || input.Items.Count == 0)
                throw new BusinessException("Order must have at least 1 item");

            Guid newOrderId = Guid.Empty;

          
            using (var outer = _uowManager.Begin(requiresNew: true, isTransactional: false))
            {
                var ctx = await _dbContextProvider.GetDbContextAsync();  
                var strategy = ctx.Database.CreateExecutionStrategy();

                await strategy.ExecuteAsync(async () =>
                {
                    
                    using var uow = _uowManager.Begin(requiresNew: true, isTransactional: false);

                    var db = await _dbContextProvider.GetDbContextAsync();
                    await using var tx = await db.Database.BeginTransactionAsync();

                    try
                    {
                        var order = new Order(GuidGenerator.Create())
                        {
                            CustomerId = input.CustomerId,
                            OrderDate = Clock.Now
                        };
                        await _orderRepo.InsertAsync(order, autoSave: true);
                        newOrderId = order.Id;

                        foreach (var i in input.Items)
                        {
                            var item = new OrderItem(GuidGenerator.Create())
                            {
                                OrderId = order.Id,
                                ProductId = i.ProductId,
                                Quantity = i.Quantity,
                                UnitPrice = i.UnitPrice
                            };
                            await _orderItemRepo.InsertAsync(item, autoSave: true);
                        }

                        await CurrentUnitOfWork.SaveChangesAsync();

                        await tx.CommitAsync();
                        await uow.CompleteAsync();
                    }
                    catch
                    {
                        await tx.RollbackAsync();
                        throw; 
                    }
                });

                await outer.CompleteAsync();
            }

            return newOrderId;
        }


        // Các mã lỗi transient thường gặp ở PostgreSQL
        private static bool IsTransient(PostgresException ex)
            => ex.SqlState is "40001"     // serialization_failure
                           or "40P01"     // deadlock_detected
                           or "55P03"     // lock_not_available
                           or "08006"     // connection_failure
                           or "08001";    // sqlclient_unable_to_establish_sqlconnection
    }
}
