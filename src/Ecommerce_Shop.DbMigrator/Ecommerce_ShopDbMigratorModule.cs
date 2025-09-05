using Ecommerce_Shop.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Ecommerce_Shop.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(Ecommerce_ShopEntityFrameworkCoreModule),
    typeof(Ecommerce_ShopApplicationContractsModule)
    )]
public class Ecommerce_ShopDbMigratorModule : AbpModule
{
}
