using Microsoft.Extensions.Localization;
using Ecommerce_Shop.Localization;
using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace Ecommerce_Shop.Web;

[Dependency(ReplaceServices = true)]
public class Ecommerce_ShopBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<Ecommerce_ShopResource> _localizer;

    public Ecommerce_ShopBrandingProvider(IStringLocalizer<Ecommerce_ShopResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
