using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce_Shop
{
    public static class ECommerceShopPermissions
    {
        public const string GroupName = "EcommerceShop";

        public static class Products
        {
            public const string Default = GroupName + ".Products";
            public const string ChangePrice = Default + ".ChangePrice";
        }

    }
}
