using Ecommerce_Shop.Dtos;
using Ecommerce_Shop.Entities;
using Ecommerce_Shop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Ecommerce_Shop
{
    public class CustomerAppService :
    CrudAppService<
        Customer,
        CustomerDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateCustomerDto,
        CreateUpdateCustomerDto>,
    ICustomerAppService
    {
        public CustomerAppService(IRepository<Customer, Guid> repo) : base(repo) { }
        // trùng email
        public override async Task<CustomerDto> CreateAsync(CreateUpdateCustomerDto input)
        {
            if (await Repository.AnyAsync(x => x.Email == input.Email))
                throw new BusinessException("CustomerEmailExists").WithData("Email", input.Email);

            return await base.CreateAsync(input);
        }

        public override async Task<CustomerDto> UpdateAsync(Guid id, CreateUpdateCustomerDto input)
        {
            if (await Repository.AnyAsync(x => x.Id != id && x.Email == input.Email))
                throw new BusinessException("CustomerEmailExists").WithData("Email", input.Email);

            return await base.UpdateAsync(id, input);
        }
    }

}
