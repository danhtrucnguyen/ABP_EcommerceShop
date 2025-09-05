using System.Threading.Tasks;

namespace Ecommerce_Shop.Data;

public interface IEcommerce_ShopDbSchemaMigrator
{
    Task MigrateAsync();
}
