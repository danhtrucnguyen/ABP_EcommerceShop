using Ecommerce_Shop.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Ecommerce_Shop.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class Ecommerce_ShopPageModel : AbpPageModel
{
    protected Ecommerce_ShopPageModel()
    {
        LocalizationResourceType = typeof(Ecommerce_ShopResource);
    }
}
