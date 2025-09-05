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
    }
}
