using FoodDelivery.Application.Interfaces;
using FoodDelivery.Application.Mappers;
using FoodDelivery.Contracts.Dtos.DishCategories;
using FoodDelivery.Domain.Entities;

namespace FoodDelivery.Application.Services;

/// <summary>
/// Сервис справочника категорий блюд
/// </summary>
public class DishCategoryService(IRepository<DishCategory> categoryRepository)
{
    /// <summary>
    /// Возвращает категорию по идентификатору
    /// </summary>
    public async Task<DishCategoryDto?> Get(int id)
    {
        var category = await categoryRepository.GetById(id);
        return category is null ? null : DishCategoryMapper.ToDto(category);
    }

    /// <summary>
    /// Возвращает список всех категорий
    /// </summary>
    public async Task<IEnumerable<DishCategoryDto>> GetAll()
    {
        var categories = await categoryRepository.GetAll();
        return categories.Select(DishCategoryMapper.ToDto);
    }
}