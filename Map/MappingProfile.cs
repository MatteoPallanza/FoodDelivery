using AutoMapper;
using FoodDelivery.Data;
using FoodDelivery.Services.Products.Commands;
using FoodDelivery.Services.UserAddresses.Commands;

namespace FoodDelivery.Map
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserAddress, CreateUserAddress>().ReverseMap();
            CreateMap<UserAddress, UpdateUserAddress>().ReverseMap();
            CreateMap<UserAddress, DeleteUserAddress>().ReverseMap();

            CreateMap<Product, CreateProduct>().ReverseMap();
            CreateMap<Product, UpdateProduct>().ReverseMap();
            CreateMap<Product, DeleteProduct>().ReverseMap();
        }
    }
}
