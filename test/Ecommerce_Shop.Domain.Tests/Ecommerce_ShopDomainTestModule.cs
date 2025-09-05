using Volo.Abp.Modularity;

namespace Ecommerce_Shop;

[DependsOn(
    typeof(Ecommerce_ShopDomainModule),
    typeof(Ecommerce_ShopTestBaseModule)
)]
public class Ecommerce_ShopDomainTestModule : AbpModule
{

}
