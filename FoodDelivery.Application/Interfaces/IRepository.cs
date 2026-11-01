namespace FoodDelivery.Application.Interfaces;

/// <summary>
/// Базовый интерфейс репозитория для CRUD-операций
/// </summary>
/// <typeparam name="T">Тип сущности</typeparam>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// Добавляет сущность и возвращает её
    /// </summary>
    Task<T> Add(T entity);

    /// <summary>
    /// Удаляет сущность по идентификатору
    /// </summary>
    Task<bool> Delete(int id);

    /// <summary>
    /// Возвращает все сущности
    /// </summary>
    Task<IList<T>> GetAll();

    /// <summary>
    /// Возвращает сущность по идентификатору
    /// </summary>
    Task<T?> GetById(int id);

    /// <summary>
    /// Обновляет сущность и возвращает её
    /// </summary>
    Task<T> Update(T entity);
}