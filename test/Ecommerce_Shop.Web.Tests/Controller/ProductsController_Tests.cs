using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

using Ecommerce_Shop.Dtos;
using Ecommerce_Shop.Services;

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
    }
}
