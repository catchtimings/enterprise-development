using FoodDelivery.Application.Interfaces.Repositories;
using FoodDelivery.Application.Interfaces.Services;
using FoodDelivery.Application.Mappers;
using FoodDelivery.Contracts.Dtos.Dishes;

namespace FoodDelivery.Application.Services;

/// <summary>
/// Сервис управления блюдами
/// </summary>
public class DishService(
    IDishRepository dishRepository,
    IDishCategoryRepository categoryRepository) : IDishService
{
    /// <summary>
    /// Создает новое блюдо
    /// </summary>
    public async Task<DishDto> Create(DishCreateUpdateDto dto)
    {
        var category = await categoryRepository.GetById(dto.CategoryId)
            ?? throw new KeyNotFoundException("Категория не найдена");

        var dish = DishMapper.Map(dto);
        dish.Category = category;

        var createdDish = await dishRepository.Add(dish);
        return DishMapper.Map(createdDish);
    }

    /// <summary>
    /// Возвращает блюдо по идентификатору
    /// </summary>
    public async Task<DishDto?> Get(int id)
    {
        var dish = await dishRepository.GetById(id);
        return dish is null ? null : DishMapper.Map(dish);
    }

    /// <summary>
    /// Возвращает список всех блюд
    /// </summary>
    public async Task<IList<DishDto>> GetAll()
    {
        var dishes = await dishRepository.GetAll();
        return DishMapper.Map(dishes.ToList());
    }

    /// <summary>
    /// Обновляет данные блюда
    /// </summary>
    public async Task<DishDto> Update(int id, DishCreateUpdateDto dto)
    {
        var existing = await dishRepository.GetById(id)
            ?? throw new KeyNotFoundException("Блюдо не найдено");

        var category = await categoryRepository.GetById(dto.CategoryId)
            ?? throw new KeyNotFoundException("Категория не найдена");

        existing.Name = dto.Name;
        existing.Weight = dto.Weight;
        existing.Price = dto.Price;
        existing.CategoryId = dto.CategoryId;
        existing.Category = category;

        var updatedDish = await dishRepository.Update(existing);
        return DishMapper.Map(updatedDish);
    }

    /// <summary>
    /// Удаляет блюдо
    /// </summary>
    public Task<bool> Delete(int id) => dishRepository.Delete(id);
}