using System;
using System.Collections.Generic;
using System.Text;
using Ecommerce_Shop.Localization;
using Volo.Abp.Application.Services;

namespace Ecommerce_Shop;

/* Inherit your application services from this class.
 */
public abstract class Ecommerce_ShopAppService : ApplicationService
{
    protected Ecommerce_ShopAppService()
    {
        LocalizationResource = typeof(Ecommerce_ShopResource);
    }
}
