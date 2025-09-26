using System.Threading.Tasks;
using OpenIddict.Abstractions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace Ecommerce_Shop.OpenIddict;

public class OpenIddictPostmanClientSeeder : IDataSeedContributor, ITransientDependency
{
    private readonly IOpenIddictApplicationManager _apps;
    private readonly IOpenIddictScopeManager _scopes;

    public OpenIddictPostmanClientSeeder(
        IOpenIddictApplicationManager apps,
        IOpenIddictScopeManager scopes)
    {
        _apps = apps;
        _scopes = scopes;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        const string apiScope = "Ecommerce_Shop";
        const string clientId = "Ecommerce_Shop_Postman";
        const string clientSecret = "1q2w3e*";

    
        if (await _scopes.FindByNameAsync(apiScope) is null)
        {
            await _scopes.CreateAsync(new OpenIddictScopeDescriptor
            {
                Name = apiScope,
                DisplayName = "Ecommerce Shop API"
            });
        }

   
        if (await _apps.FindByClientIdAsync(clientId) is not null)
            return;

        var desc = new OpenIddictApplicationDescriptor
        {
            ClientId = clientId,
            ClientSecret = clientSecret, 
            DisplayName = "Postman Client",
            ClientType = OpenIddictConstants.ClientTypes.Confidential
        };

        // endpoints
        desc.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Token);

        // grant types
        desc.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.Password);
        desc.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.RefreshToken);

        // scopes 
        desc.Permissions.Add(OpenIddictConstants.Permissions.Prefixes.Scope + "offline_access");
        desc.Permissions.Add(OpenIddictConstants.Permissions.Prefixes.Scope + apiScope);

        await _apps.CreateAsync(desc);
    }
}
