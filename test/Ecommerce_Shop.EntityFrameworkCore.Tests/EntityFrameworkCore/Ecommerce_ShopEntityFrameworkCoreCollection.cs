using Xunit;

namespace Ecommerce_Shop.EntityFrameworkCore;

[CollectionDefinition(Ecommerce_ShopTestConsts.CollectionDefinitionName)]
public class Ecommerce_ShopEntityFrameworkCoreCollection : ICollectionFixture<Ecommerce_ShopEntityFrameworkCoreFixture>
{

}
