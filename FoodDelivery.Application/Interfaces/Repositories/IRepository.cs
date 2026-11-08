namespace FoodDelivery.Application.Interfaces.Repositories;

/// <summary>
/// Базовый интерфейс репозитория для CRUD-операций
/// </summary>
/// <typeparam name="TEntity">Тип сущности</typeparam>
/// <typeparam name="TKey">Тип идентификатора сущности</typeparam>
public interface IRepository<TEntity, TKey> where TEntity : class
{
    /// <summary>
    /// Добавление сущности
    /// </summary>
    /// <param name="entity">Сущность</param>
    /// <returns>Сущность</returns>
    public Task<TEntity> Add(TEntity entity);

    /// <summary>
    /// Удаление сущности по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    /// <returns>True, если сущность была удалена, иначе False</returns>
    public Task<bool> Delete(TKey id);

    /// <summary>
    /// Возвращает все сущности
    /// </summary>
    /// <returns>Список сущностей</returns>
    public Task<IList<TEntity>> GetAll();

    /// <summary>
    /// Возвращает сущность по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    /// <returns>Сущность или null, если не найдена</returns>   
    public Task<TEntity?> GetById(TKey id);

    /// <summary>
    /// Обновляет сущность и возвращает её
    /// </summary>
    /// <param name="entity">Сущность</param>
    /// <returns>Обновленная</returns>
    public Task<TEntity> Update(TEntity entity);
}