using Sevriukoff.ProjectManager.Infrastructure.Interfaces;

namespace Sevriukoff.ProjectManager.Infrastructure.Base;

/// <summary>
/// Базовый класс, представляющий сущность с уникальным идентификатором.
/// </summary>
/// <typeparam name="T">Тип идентификатора.</typeparam>
public abstract class BaseEntity<T> : IBaseEntity<T>
{
    /// <summary>
    /// Получает или задает уникальный идентификатор сущности.
    /// </summary>
    public T Id { get; set; }
    
    // Пример других свойств.
    // public DateTime? CreatedAt { get; set; }
    // public T UserIdCreated { get; set; }
    // public DateTime? AtModified { get; set; }
    // public T UserIdModified { get; set; }
}