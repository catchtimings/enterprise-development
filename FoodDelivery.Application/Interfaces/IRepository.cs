namespace FoodDelivery.Application.Interfaces;

/// <summary>
/// Базовый интерфейс репозитория для CRUD-операций
/// </summary>
public interface IRepository<T> where T : class
{
    /// <summary>Добавляет сущность</summary>
    public Task Add(T entity);

    /// <summary>
    /// Удаляет сущность по идентификатору
    /// </summary>
    public Task Delete(int id);

    /// <summary>
    /// Возвращает все сущности
    /// </summary>
    public Task<IList<T>> GetAll();

    /// <summary>
    /// Возвращает сущность по идентификатору
    /// </summary>
    public Task<T?> GetById(int id);

    /// <summary>
    /// Проверяет существование сущности
    /// </summary>
    public Task<bool> IsExists(int id);

    /// <summary>
    /// Обновляет сущность
    /// </summary>
    public Task Update(T entity);
}