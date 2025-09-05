using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Ecommerce_Shop.Data;

/* This is used if database provider does't define
 * IEcommerce_ShopDbSchemaMigrator implementation.
 */
public class NullEcommerce_ShopDbSchemaMigrator : IEcommerce_ShopDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
