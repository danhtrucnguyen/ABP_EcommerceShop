using Ecommerce_Shop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Specifications;

namespace Ecommerce_Shop.Specifications
{
    public sealed class OrderWithDetailsSpecification : Specification<Order>
    {
        private readonly Guid _id;
        public OrderWithDetailsSpecification(Guid id) => _id = id;

        public override Expression<Func<Order, bool>> ToExpression()
            => o => o.Id == _id;
    }
}
