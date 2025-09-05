using Ecommerce_Shop.Samples;
using Xunit;

namespace Ecommerce_Shop.EntityFrameworkCore.Domains;

[Collection(Ecommerce_ShopTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<Ecommerce_ShopEntityFrameworkCoreTestModule>
{

}
