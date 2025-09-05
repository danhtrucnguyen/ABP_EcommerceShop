using Ecommerce_Shop.Samples;
using Xunit;

namespace Ecommerce_Shop.EntityFrameworkCore.Applications;

[Collection(Ecommerce_ShopTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<Ecommerce_ShopEntityFrameworkCoreTestModule>
{

}
