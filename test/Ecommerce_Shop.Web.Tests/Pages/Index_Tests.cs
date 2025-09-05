using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Ecommerce_Shop.Pages;

public class Index_Tests : Ecommerce_ShopWebTestBase
{
    [Fact]
    public async Task Welcome_Page()
    {
        var response = await GetResponseAsStringAsync("/");
        response.ShouldNotBeNull();
    }
}
