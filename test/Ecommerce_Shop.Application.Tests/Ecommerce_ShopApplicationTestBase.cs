using Volo.Abp.Modularity;

namespace Ecommerce_Shop;

public abstract class Ecommerce_ShopApplicationTestBase<TStartupModule> : Ecommerce_ShopTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
