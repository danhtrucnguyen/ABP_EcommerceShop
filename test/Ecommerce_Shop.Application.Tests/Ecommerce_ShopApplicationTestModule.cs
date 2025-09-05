using Volo.Abp.Modularity;

namespace Ecommerce_Shop;

[DependsOn(
    typeof(Ecommerce_ShopApplicationModule),
    typeof(Ecommerce_ShopDomainTestModule)
)]
public class Ecommerce_ShopApplicationTestModule : AbpModule
{

}
