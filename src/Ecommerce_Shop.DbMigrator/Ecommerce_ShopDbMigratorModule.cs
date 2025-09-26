using Ecommerce_Shop.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace Ecommerce_Shop.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(Ecommerce_ShopEntityFrameworkCoreModule),
    typeof(Ecommerce_ShopApplicationContractsModule)
    )]
public class Ecommerce_ShopDbMigratorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var cfg = context.Services.GetConfiguration();
        var cs = cfg.GetConnectionString("Default"); // đọc từ appsettings/secrets

        // 🔒 NẾU vẫn null/empty, đặt tạm chuỗi fallback ở đây để chạy được ngay
        if (string.IsNullOrWhiteSpace(cs))
        {
            cs = "Host=localhost;Port=5432;Database=Ecommerce_Shop;Username=postgres;Password=123456;Pooling=true";
        }

        // Ép ABP dùng đúng connection string này cho mọi DbContext khi chạy DbMigrator
        Configure<AbpDbConnectionOptions>(o =>
        {
            o.ConnectionStrings.Default = cs;
        });
    }
}
