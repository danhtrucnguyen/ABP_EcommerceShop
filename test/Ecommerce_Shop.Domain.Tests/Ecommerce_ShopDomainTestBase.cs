using Volo.Abp.Modularity;

namespace Ecommerce_Shop;

/* Inherit from this class for your domain layer tests. */
public abstract class Ecommerce_ShopDomainTestBase<TStartupModule> : Ecommerce_ShopTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
