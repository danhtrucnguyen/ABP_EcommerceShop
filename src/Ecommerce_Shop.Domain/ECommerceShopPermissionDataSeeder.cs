
using System;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;

namespace Ecommerce_Shop;

public class ECommerceShopPermissionDataSeeder : IDataSeedContributor, ITransientDependency
{
    private readonly IdentityRoleManager _roleManager;
    private readonly IPermissionDataSeeder _permissionDataSeeder;
    private readonly IGuidGenerator _guidGenerator;

    public ECommerceShopPermissionDataSeeder(
        IdentityRoleManager roleManager,
        IPermissionDataSeeder permissionDataSeeder,
        IGuidGenerator guidGenerator)
    {
        _roleManager = roleManager;
        _permissionDataSeeder = permissionDataSeeder;
        _guidGenerator = guidGenerator;
    }

    [UnitOfWork]
    public virtual async Task SeedAsync(DataSeedContext context)
    {
        var roleName = "product-manager";

        var role = await _roleManager.FindByNameAsync(roleName);
        if (role == null)
        {
            role = new IdentityRole(_guidGenerator.Create(), roleName, context.TenantId);

            
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                var reason = string.Join("; ", result.Errors);
                throw new Exception($"Create role '{roleName}' failed: {reason}");
            }
        }

       
        await _permissionDataSeeder.SeedAsync(
            role.Name,                                        // tên role
            RolePermissionValueProvider.ProviderName,         // providerName = cấp cho role
            new[]
            {
                     ECommerceShopPermissions.Products.ChangePrice
            },
            context.TenantId
        );

    }
}
