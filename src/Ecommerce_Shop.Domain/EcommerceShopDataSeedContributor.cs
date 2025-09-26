using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;
using Ecommerce_Shop.Permissions;

public class EcommerceShopDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IdentityRoleManager _roleManager;
    private readonly IdentityUserManager _userManager;
    private readonly IIdentityRoleRepository _roleRepo;
    private readonly IIdentityUserRepository _userRepo;
    private readonly IPermissionManager _permissionManager;
    private readonly IGuidGenerator _guidGenerator;

    public EcommerceShopDataSeedContributor(
        IdentityRoleManager roleManager,
        IdentityUserManager userManager,
        IIdentityRoleRepository roleRepo,
        IIdentityUserRepository userRepo,
        IPermissionManager permissionManager,
        IGuidGenerator guidGenerator)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _roleRepo = roleRepo;
        _userRepo = userRepo;
        _permissionManager = permissionManager;
        _guidGenerator = guidGenerator;
    }

    [UnitOfWork]
    public async Task SeedAsync(DataSeedContext context)
    {
        // Roles
        var managerRole = await _roleRepo.FindByNormalizedNameAsync("MANAGER")
                         ?? new IdentityRole(_guidGenerator.Create(), "Manager");
        if (managerRole.Id == default) (await _roleManager.CreateAsync(managerRole)).CheckErrors();

        var staffRole = await _roleRepo.FindByNormalizedNameAsync("STAFF")
                       ?? new IdentityRole(_guidGenerator.Create(), "Staff");
        if (staffRole.Id == default) (await _roleManager.CreateAsync(staffRole)).CheckErrors();

        // Grant permissions
        await GrantAll(managerRole.Name);
        await _permissionManager.SetForRoleAsync(staffRole.Name, EcommerceShopPermissions.Products.Default, true);
        await _permissionManager.SetForRoleAsync(staffRole.Name, EcommerceShopPermissions.Products.Create, true);
        await _permissionManager.SetForRoleAsync(staffRole.Name, EcommerceShopPermissions.Products.Update, true);

        // Users
        var manager = await _userRepo.FindByNormalizedUserNameAsync("MANAGER1")
                      ?? new IdentityUser(_guidGenerator.Create(), "manager1", "manager1@local.test");
        if (manager.Id == default)
        {
            (await _userManager.CreateAsync(manager, "Abp123!")).CheckErrors();
            (await _userManager.AddToRoleAsync(manager, managerRole.Name)).CheckErrors();
        }

        var staff = await _userRepo.FindByNormalizedUserNameAsync("STAFF1")
                    ?? new IdentityUser(_guidGenerator.Create(), "staff1", "staff1@local.test");
        if (staff.Id == default)
        {
            (await _userManager.CreateAsync(staff, "Abp123!")).CheckErrors();
            (await _userManager.AddToRoleAsync(staff, staffRole.Name)).CheckErrors();
        }
    }

    private async Task GrantAll(string roleName)
    {
        // Products
        await _permissionManager.SetForRoleAsync(roleName, EcommerceShopPermissions.Products.Default, true);
        await _permissionManager.SetForRoleAsync(roleName, EcommerceShopPermissions.Products.Create, true);
        await _permissionManager.SetForRoleAsync(roleName, EcommerceShopPermissions.Products.Update, true);
        await _permissionManager.SetForRoleAsync(roleName, EcommerceShopPermissions.Products.Delete, true);
        // Categories
        await _permissionManager.SetForRoleAsync(roleName, EcommerceShopPermissions.Categories.Default, true);
        await _permissionManager.SetForRoleAsync(roleName, EcommerceShopPermissions.Categories.Create, true);
        await _permissionManager.SetForRoleAsync(roleName, EcommerceShopPermissions.Categories.Update, true);
        await _permissionManager.SetForRoleAsync(roleName, EcommerceShopPermissions.Categories.Delete, true);
        // Customers
        await _permissionManager.SetForRoleAsync(roleName, EcommerceShopPermissions.Customers.Default, true);
        await _permissionManager.SetForRoleAsync(roleName, EcommerceShopPermissions.Customers.Create, true);
        await _permissionManager.SetForRoleAsync(roleName, EcommerceShopPermissions.Customers.Update, true);
        await _permissionManager.SetForRoleAsync(roleName, EcommerceShopPermissions.Customers.Delete, true);
    }
}
