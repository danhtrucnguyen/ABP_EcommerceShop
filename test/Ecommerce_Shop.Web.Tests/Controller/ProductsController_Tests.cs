using Ecommerce_Shop.Dtos;
using Ecommerce_Shop.Services;
using Shouldly;
using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Xunit;

namespace Ecommerce_Shop.Web.Tests.Controller
{
    public class ProductsController_Tests : Ecommerce_ShopWebTestBase
    {
        private readonly ICustomerAppService _customerApp;
        private readonly IProductAppService _productApp;
        private readonly IOrderAppService _orderApp;

        public ProductsController_Tests()
        {
            _customerApp = GetRequiredService<ICustomerAppService>();
            _productApp = GetRequiredService<IProductAppService>();
            _orderApp = GetRequiredService<IOrderAppService>();
        }

        [Fact]
        public async Task GetNeverOrdered_Should_Return_Product()
        {
            var p = await _productApp.CreateAsync(new CreateUpdateProductDto
            {
                Name = "NeverOrder",
                Price = 10m
            });

            var client = CreateClient();
            var resp = await client.GetAsync("/api/products/never-ordered");

            resp.StatusCode.ShouldBe(HttpStatusCode.OK);

            var result = await resp.Content.ReadFromJsonAsync<ProductDto[]>();
            result.ShouldNotBeNull();
            result.ShouldContain(x => x.Id == p.Id);
        }

        [Fact]
        public async Task GetPurchasedByCustomer_Should_Filter_Correctly()
        {
            var customer = await _customerApp.CreateAsync(new CreateUpdateCustomerDto
            {
                Name = "Cus",
                Email = "cus@test.com",
                Phone = "0909",
                Address = "HN"
            });

            var product = await _productApp.CreateAsync(new CreateUpdateProductDto
            {
                Name = "P1",
                Price = 100m
            });

            _ = await _orderApp.CreateAsync(new CreateOrderDto
            {
                CustomerId = customer.Id,
                Items =
                {
                    new CreateOrderItemDto
                    {
                        ProductId = product.Id,
                        Quantity  = 1,
                        UnitPrice = 100m
                    }
                }
            });

            var client = CreateClient();
            var fromDate = DateTime.UtcNow.AddDays(-1).ToString("O");
            var toDate = DateTime.UtcNow.AddDays(1).ToString("O");

            var url = $"/api/products/purchased-by-customer?customerId={customer.Id}&fromDate={fromDate}&toDate={toDate}";
            var resp = await client.GetAsync(url);

            resp.StatusCode.ShouldBe(HttpStatusCode.OK);

            var result = await resp.Content.ReadFromJsonAsync<ProductDto[]>();
            result.ShouldNotBeNull();
            result.ShouldContain(x => x.Id == product.Id);
        }

        [Fact]
        public async Task AllIncludingDeleted_Should_Return_SoftDeleted_Item()
        {
            var p = await _productApp.CreateAsync(new CreateUpdateProductDto
            {
                Name = "SoftDel-" + Guid.NewGuid().ToString("N").Substring(0, 6),
                Price = 11m
            });
            await _productApp.DeleteAsync(p.Id);

            var client = CreateClient();

        
            var listResp = await client.GetAsync("/api/products?SkipCount=0&MaxResultCount=50");
            listResp.StatusCode.ShouldBe(HttpStatusCode.OK);
            var list = await listResp.Content.ReadFromJsonAsync<PagedResultDto<ProductDto>>();
            list.ShouldNotBeNull();
            list.Items.ShouldNotContain(x => x.Id == p.Id);

           
            var allResp = await client.GetAsync("/api/products/all-including-deleted");
            allResp.StatusCode.ShouldBe(HttpStatusCode.OK);
            var all = await allResp.Content.ReadFromJsonAsync<ProductDto[]>();
            all.ShouldNotBeNull();
            all.ShouldContain(x => x.Id == p.Id);
        }

        [Fact]
        public async Task Deleted_Endpoint_Should_Return_Only_SoftDeleted_Items()
        {
            // add 2 product, xóa mềm 1 cái
            var p1 = await _productApp.CreateAsync(new CreateUpdateProductDto { Name = "A-" + Guid.NewGuid().ToString("N").Substring(0, 6), Price = 5m });
            var p2 = await _productApp.CreateAsync(new CreateUpdateProductDto { Name = "B-" + Guid.NewGuid().ToString("N").Substring(0, 6), Price = 6m });
            await _productApp.DeleteAsync(p2.Id); 

            var client = CreateClient();

            
            var resp = await client.GetAsync("/api/products/deleted");

           
            resp.StatusCode.ShouldBe(HttpStatusCode.OK);
            var result = await resp.Content.ReadFromJsonAsync<ProductDto[]>();
            result.ShouldNotBeNull();

            result.ShouldContain(x => x.Id == p2.Id);   // đã xóa mềm , xuất hiện
            result.ShouldNotContain(x => x.Id == p1.Id); // chưa xóa , không xuất hiện
        }
    }
}

