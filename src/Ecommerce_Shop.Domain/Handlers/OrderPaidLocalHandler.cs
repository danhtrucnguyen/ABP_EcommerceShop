using Ecommerce_Shop.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;
using Volo.Abp.Uow;

namespace Ecommerce_Shop.Handlers
{
    public class OrderPaidLocalHandler : 
        ILocalEventHandler<OrderPaidEvent>,
        ITransientDependency
    {
        private readonly ILogger<OrderPaidLocalHandler> _logger;

        public OrderPaidLocalHandler(ILogger<OrderPaidLocalHandler> logger)
        {
            _logger = logger;
        }
        public Task HandleEventAsync(OrderPaidEvent eventData)
        {
            _logger.LogInformation(
                "[LOCAL] Order {OrderId} has been PAID at {PaidAt}. Total={Total}",
                eventData.OrderId, eventData.PaidAt, eventData.TotalAmount);

            return Task.CompletedTask;
        }
    }
}
