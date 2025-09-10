using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce_Shop
{
    using Ecommerce_Shop.Entities;
    using Shouldly;
    using System;
    using Xunit;

    

    public class Product_Tests
    {
        [Fact]
        public void Ctor_Should_Set_Properties()
        {
            var id = Guid.NewGuid();
            var p = new Product(id, "Name", 123.45m, Guid.Empty);

            p.Id.ShouldBe(id);
            p.Name.ShouldBe("Name");
            p.Price.ShouldBe(123.45m);
            p.Description.ShouldBeNull();
            p.CategoryId.ShouldBe(Guid.Empty);
        }

        [Fact]
        public void Update_Should_Change_Properties()
        {
            var p = new Product(Guid.NewGuid(), "N1", 10m, null);
            var newCat = Guid.NewGuid();
            p.Update("N2", 20m, "d2", newCat);

            p.Name.ShouldBe("N2");
            p.Price.ShouldBe(20m);
            p.Description.ShouldBe("d2");
            p.CategoryId.ShouldBe(newCat);
        }
    }

}
