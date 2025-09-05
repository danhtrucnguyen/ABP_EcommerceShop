using Ecommerce_Shop.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Ecommerce_Shop.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class Ecommerce_ShopController : AbpControllerBase
{
    protected Ecommerce_ShopController()
    {
        LocalizationResource = typeof(Ecommerce_ShopResource);
    }
}
