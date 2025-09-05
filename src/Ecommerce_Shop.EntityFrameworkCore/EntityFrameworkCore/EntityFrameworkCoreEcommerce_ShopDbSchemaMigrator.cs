using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ecommerce_Shop.Data;
using Volo.Abp.DependencyInjection;

namespace Ecommerce_Shop.EntityFrameworkCore;

public class EntityFrameworkCoreEcommerce_ShopDbSchemaMigrator
    : IEcommerce_ShopDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreEcommerce_ShopDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the Ecommerce_ShopDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<Ecommerce_ShopDbContext>()
            .Database
            .MigrateAsync();
    }
}
