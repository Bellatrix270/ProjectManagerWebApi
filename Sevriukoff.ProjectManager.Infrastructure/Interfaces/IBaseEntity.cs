namespace Sevriukoff.ProjectManager.Infrastructure.Interfaces;

/// <summary>
/// Интерфейс, определяющий основную структуру сущности с уникальным идентификатором.
/// </summary>
/// <typeparam name="T">Тип идентификатора.</typeparam>
public interface IBaseEntity<T>
{
    /// <summary>
    /// Получает или задает уникальный идентификатор сущности.
    /// </summary>
    T Id { get; set; }
}