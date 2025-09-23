using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Ecommerce_Shop.Dtos
{
    public class ReviewDto : EntityDto<Guid>
    {
        public Guid ProductId { get; set; }
        public Guid CustomerId { get; set; }
        public byte Rating { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsApproved { get; set; }
    }

    public class CreateUpdateReviewDto
    {
        public Guid ProductId { get; set; }
        public Guid CustomerId { get; set; }
        public byte Rating { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
