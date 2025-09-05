using Volo.Abp.Settings;

namespace Ecommerce_Shop.Settings;

public class Ecommerce_ShopSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(Ecommerce_ShopSettings.MySetting1));
    }
}
