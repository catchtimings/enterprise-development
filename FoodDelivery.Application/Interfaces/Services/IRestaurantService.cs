using FoodDelivery.Contracts.Dtos.Restaurants;

namespace FoodDelivery.Application.Interfaces.Services;

public interface IRestaurantService : IApplicationService<RestaurantDto, RestaurantCreateUpdateDto, int>;