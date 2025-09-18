using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce_Shop.Dtos
{
    public class LocalDataSnapshotDto
    {
        public List<Guid> TrackedOrders { get; set; } = new();
        public List<TrackedItemDto> TrackedItems { get; set; } = new();
        public List<Guid> TrackedProducts { get; set; } = new();
        public int PendingChangesCount { get; set; }
    }

    public class TrackedItemDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }

}
