using Ecommerce_Shop.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Ecommerce_Shop.Permissions;

public class Ecommerce_ShopPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(Ecommerce_ShopPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(Ecommerce_ShopPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<Ecommerce_ShopResource>(name);
    }
}
