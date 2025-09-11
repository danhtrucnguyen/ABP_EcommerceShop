using Ecommerce_Shop.Domain.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;

namespace Ecommerce_Shop.Handlers
{
    public class ProductPriceChangedHandler :
        ILocalEventHandler<ProductPriceChangedEvent>,
        ITransientDependency
    {
        private readonly ILogger<ProductPriceChangedHandler> _logger;

        public ProductPriceChangedHandler(ILogger<ProductPriceChangedHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleEventAsync(ProductPriceChangedEvent eventData)
        {
            _logger.LogInformation(
                "[LOCAL] Product {ProductId} price changed {OldPrice} -> {NewPrice} at {Time}",
                eventData.ProductId, eventData.OldPrice, eventData.NewPrice, eventData.ChangeTime);

            return Task.CompletedTask;
        }
    }
}
