using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Authorization.Permissions;

namespace Ecommerce_Shop.Permissions
{
    public class EcommerceShopRolePermissionSeeder : IDataSeedContributor, ITransientDependency
    {
        private readonly IPermissionDataSeeder _permissionDataSeeder;

        public EcommerceShopRolePermissionSeeder(IPermissionDataSeeder permissionDataSeeder)
        {
            _permissionDataSeeder = permissionDataSeeder;
        }

        public async Task SeedAsync (DataSeedContext context)
        {
            var managerPerms = new[]
            {
                EcommerceShopPermissions.Products.Default,
                EcommerceShopPermissions.Products.Update,
                EcommerceShopPermissions.Categories.Default,
                EcommerceShopPermissions.Orders.Default,
                EcommerceShopPermissions.Orders.Update
            };

            await _permissionDataSeeder.SeedAsync(
                RolePermissionValueProvider.ProviderName,
                "Manager",
                managerPerms
                );

            var staffPerms = new[]
           {
                EcommerceShopPermissions.Orders.Default,
                EcommerceShopPermissions.Orders.Create
            };

            await _permissionDataSeeder.SeedAsync(
                RolePermissionValueProvider.ProviderName,
                "Staff",
                staffPerms
            );
        }
    }
}
