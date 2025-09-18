using Ecommerce_Shop.Localization;
using Localization.Resources.AbpUi;
using Volo.Abp.Account;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace Ecommerce_Shop;

[DependsOn(
    typeof(Ecommerce_ShopApplicationContractsModule),
    typeof(AbpAccountHttpApiModule),
    typeof(AbpIdentityHttpApiModule),
    typeof(AbpPermissionManagementHttpApiModule),
    typeof(AbpTenantManagementHttpApiModule),
    typeof(AbpFeatureManagementHttpApiModule),
    typeof(AbpSettingManagementHttpApiModule)
    )]
public class Ecommerce_ShopHttpApiModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        ConfigureLocalization();
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            // <<< QUAN TRỌNG: trỏ tới assembly của Application module >>>
            options.ConventionalControllers.Create(typeof(Ecommerce_ShopApplicationModule).Assembly);
        });
    }

    private void ConfigureLocalization()
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<Ecommerce_ShopResource>()
                .AddBaseTypes(
                    typeof(AbpUiResource)
                );
        });
    }
}
