using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;

namespace Ecommerce_Shop.Identity
{

    public class EcommerceShopUserSeeder : IDataSeedContributor, ITransientDependency
    {
        private readonly IdentityUserManager _userManager;
        private readonly IdentityRoleManager _roleManager;
        private readonly IGuidGenerator _guid;

        public EcommerceShopUserSeeder(
            IdentityUserManager userManager,
            IdentityRoleManager roleManager,
            IGuidGenerator guid)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _guid = guid;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            
            await EnsureRoleAsync("Manager", context);
            await EnsureRoleAsync("Staff", context);

           
            await EnsureUserAsync(
                userName: "manager1",
                email: "manager1@local.com",
                password: "1q2w3E*",
                roleName: "Manager",
                context: context
            );

            await EnsureUserAsync(
                userName: "staff1",
                email: "staff1@local.com",
                password: "1q2w3E*",
                roleName: "Staff",
                context: context
            );
        }

        private async Task EnsureRoleAsync(string roleName, DataSeedContext context)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null) return;

            role = new IdentityRole(_guid.Create(), roleName, context.TenantId);
            (await _roleManager.CreateAsync(role)).CheckErrors();
        }

        private async Task EnsureUserAsync(string userName, string email, string password, string roleName, DataSeedContext context)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                
                if (!await _userManager.IsInRoleAsync(user, roleName))
                {
                    (await _userManager.AddToRoleAsync(user, roleName)).CheckErrors();
                }
                return;
            }

            user = new IdentityUser(_guid.Create(), userName, email, context.TenantId);
            (await _userManager.CreateAsync(user, password)).CheckErrors();
            (await _userManager.AddToRoleAsync(user, roleName)).CheckErrors();
        }
    }
}
