using Ecommerce_Shop.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Ecommerce_Shop.Permissions
{
    public class EcommerceShopPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var group = context.AddGroup(EcommerceShopPermissions.GroupName, L("Permission:Ecommerce"));

            var products = group.AddPermission(EcommerceShopPermissions.Products.Default, L("Permission:Products"));
            products.AddChild(EcommerceShopPermissions.Products.Create, L("Permission:Products.Create"));
            products.AddChild(EcommerceShopPermissions.Products.Update, L("Permission:Products.Update"));
            products.AddChild(EcommerceShopPermissions.Products.Delete, L("Permission:Products.Delete"));

            var categories = group.AddPermission(EcommerceShopPermissions.Categories.Default, L("Permission:Categories"));
            categories.AddChild(EcommerceShopPermissions.Categories.Create, L("Permission:Categories.Create"));
            categories.AddChild(EcommerceShopPermissions.Categories.Update, L("Permission:Categories.Update"));
            categories.AddChild(EcommerceShopPermissions.Categories.Delete, L("Permission:Categories.Delete"));

            var customers = group.AddPermission(EcommerceShopPermissions.Customers.Default, L("Permission:Customers"));
            customers.AddChild(EcommerceShopPermissions.Customers.Create, L("Permission:Customers.Create"));
            customers.AddChild(EcommerceShopPermissions.Customers.Update, L("Permission:Customers.Update"));
            customers.AddChild(EcommerceShopPermissions.Customers.Delete, L("Permission:Customers.Delete"));

            var orders = group.AddPermission(EcommerceShopPermissions.Orders.Default, L("Permission:Orders"));
            orders.AddChild(EcommerceShopPermissions.Orders.Create, L("Permission:Orders.Create"));
            orders.AddChild(EcommerceShopPermissions.Orders.Update, L("Permission:Orders.Update"));
            orders.AddChild(EcommerceShopPermissions.Orders.Delete, L("Permission:Orders.Delete"));

        }

        private static LocalizableString L(string name) => LocalizableString.Create<Ecommerce_ShopResource>(name);
    }
}
