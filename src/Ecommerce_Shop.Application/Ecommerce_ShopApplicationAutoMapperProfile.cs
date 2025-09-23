using AutoMapper;
using Ecommerce_Shop.Dtos;
using Ecommerce_Shop.Entities;

namespace Ecommerce_Shop;

public class Ecommerce_ShopApplicationAutoMapperProfile : Profile
{
    public Ecommerce_ShopApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap<Product, ProductDto>(); CreateMap<CreateUpdateProductDto, Product>();

        CreateMap<Category, CategoryDto>(); CreateMap<CreateUpdateCategoryDto, Category>();

        CreateMap<Customer, CustomerDto>(); CreateMap<CreateUpdateCustomerDto, Customer>();

        CreateMap<Order, OrderDto>()
            .ForMember(d => d.CustomerName, opt => opt.MapFrom(s => s.Customer != null ? s.Customer.Name : null))
            .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()));

        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(d => d.ProductName, opt => opt.MapFrom(s => s.Product != null ? s.Product.Name : null));

        CreateMap<Review, ReviewDto>();
        CreateMap<CreateUpdateReviewDto, Review>();
    }
}
