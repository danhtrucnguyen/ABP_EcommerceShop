using Microsoft.AspNetCore.Builder;
using Ecommerce_Shop;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();

builder.Environment.ContentRootPath = GetWebProjectContentRootPathHelper.Get("Ecommerce_Shop.Web.csproj");
await builder.RunAbpModuleAsync<Ecommerce_ShopWebTestModule>(applicationName: "Ecommerce_Shop.Web" );

public partial class Program
{
}
