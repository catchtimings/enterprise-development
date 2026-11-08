using FoodDelivery.Application.Interfaces.Repositories;
using FoodDelivery.Application.Interfaces.Services;
using FoodDelivery.Application.Mappers;
using FoodDelivery.Contracts.Dtos.DishCategories;

namespace FoodDelivery.Application.Services;

/// <summary>
/// Сервис справочника категорий блюд
/// </summary>
public class DishCategoryService(IDishCategoryRepository categoryRepository) : IDishCategoryService
{
    /// <summary>
    /// Создает новую категорию
    /// </summary>
    public async Task<DishCategoryDto> Create(DishCategoryCreateUpdateDto dto)
    {
        var category = DishCategoryMapper.Map(dto);
        var createdCategory = await categoryRepository.Add(category);
        return DishCategoryMapper.Map(createdCategory);
    }

    /// <summary>
    /// Возвращает категорию по идентификатору
    /// </summary>
    public async Task<DishCategoryDto?> Get(int id)
    {
        var category = await categoryRepository.GetById(id);
        return category is null ? null : DishCategoryMapper.Map(category);
    }

    /// <summary>
    /// Возвращает список всех категорий
    /// </summary>
    public async Task<IList<DishCategoryDto>> GetAll()
    {
        var categories = await categoryRepository.GetAll();
        return DishCategoryMapper.Map(categories.ToList());
    }

    /// <summary>
    /// Обновляет данные категории
    /// </summary>
    public async Task<DishCategoryDto> Update(int id, DishCategoryCreateUpdateDto dto)
    {
        var existing = await categoryRepository.GetById(id)
            ?? throw new KeyNotFoundException("Категория не найдена");

        existing.Name = dto.Name;

        var updatedCategory = await categoryRepository.Update(existing);
        return DishCategoryMapper.Map(updatedCategory);
    }

    /// <summary>
    /// Удаляет категорию
    /// </summary>
    public Task<bool> Delete(int id) => categoryRepository.Delete(id);
}