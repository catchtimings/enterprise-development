namespace FoodDelivery.Application.Interfaces.Services;

///<summary>
/// Интерфейс для сервисов приложения
/// </summary>
/// <typeparam name="TDto"> DTO для возврата сущности</typeparam>
/// <typeparam name="TCreateUpdateDto"> DTO для создания и обновления сущности</typeparam>
/// <typeparam name="Tkey"> Тип идентификатора сущности</typeparam>
public interface IApplicationService<TDto, TCreateUpdateDto, Tkey>
{
    /// <summary>
    /// Создание новой сущности
    /// </summary>
    /// <param name="dto">Данны для создания сущности</param>
    /// <returns>Созданная сущность в виде DTO</returns>
    public Task<TDto> Create(TCreateUpdateDto dto);

    /// <summary>
    /// Получение сущности по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    /// <returns>Сущность в виде DTO или null, если не найдена</returns>
    public Task<TDto?> Get(Tkey id);

    /// <summary>
    /// Получение всех сущностей
    /// </summary>
    /// <returns>Список сущностей в виде DTO</returns>
    public Task<IList<TDto>> GetAll();

    /// <summary>
    /// Обновление сущности по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    /// <param name="dto">DTO для обновления сущности</param>
    /// <returns>Обновленная сущность в виде DTO</returns>
    public Task<TDto> Update(Tkey id, TCreateUpdateDto dto);

    /// <summary>
    /// Удаление сущности по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    /// <returns>True, если сущность была удалена, иначе False</returns>
    public Task<bool> Delete(Tkey id);
}
