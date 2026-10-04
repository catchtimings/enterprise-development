using AutoMapper;
using FoodDelivery.Contracts.Dtos;
using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Contracts.Mapper;

/// <summary>
/// Профиль AutoMapper для сопоставления доменных моделей и DTO
/// </summary>
public class AppMapper : Profile
{
    /// <summary>
    /// Инициализирует сопоставления между сущностями и DTO
    /// </summary>
    public AppMapper()
    {
        CreateMap<Restaurant, RestaurantDto>().ReverseMap();
        CreateMap<RestaurantCreateDto, Restaurant>();

        CreateMap<Client, ClientDto>().ReverseMap();
        CreateMap<ClientCreateDto, Client>();

        CreateMap<DishCategory, DishCategoryDto>().ReverseMap();

        CreateMap<Dish, DishDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null));
        CreateMap<DishCreateDto, Dish>();

        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(dest => dest.DishName, opt => opt.MapFrom(src => src.Dish != null ? src.Dish.Name : null));
        CreateMap<OrderItemCreateDto, OrderItem>();

        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Client != null ? src.Client.FullName : null))
            .ForMember(dest => dest.RestaurantName, opt => opt.MapFrom(src => src.Restaurant != null ? src.Restaurant.Name : null));
        CreateMap<OrderCreateDto, Order>();
    }
}