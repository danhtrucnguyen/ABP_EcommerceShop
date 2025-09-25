using Ecommerce_Shop.Events.Eto;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace Ecommerce_Shop.Handlers
{
    public class OrderCreatedDistributedHandler :
        IDistributedEventHandler<OrderCreatedEto>,
        ITransientDependency
    {
        private readonly ILogger<OrderCreatedDistributedHandler> _logger;

        public OrderCreatedDistributedHandler(ILogger<OrderCreatedDistributedHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleEventAsync(OrderCreatedEto eventData)
        {
            _logger.LogInformation(
                "[DIST] Received OrderCreatedEto: Order={OrderId}, Customer={CustomerId}, Total={Total}, Items={Count}",
                eventData.OrderId, eventData.CustomerId, eventData.TotalAmount, eventData.Items?.Count ?? 0);

           
            return Task.CompletedTask;
        }
    }
}
