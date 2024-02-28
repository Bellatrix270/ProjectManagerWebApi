using System.ComponentModel.DataAnnotations;
using Sevriukoff.ProjectManager.Infrastructure.Entities;

namespace Sevriukoff.ProjectManager.WebApi.Helpers;

/// <summary>
/// Параметры запроса для задач проекта.
/// </summary>
public class ProjectTaskQueryParameters
{
    /// <summary>
    /// Статус задачи.
    /// </summary>
    public ProjectTaskStatus? Status { get; set; }

    /// <summary>
    /// Приоритет задачи.
    /// </summary>
    [Range(1, 10)]
    public int? Priority { get; set; }

    /// <summary>
    /// Идентификатор создателя задачи.
    /// </summary>
    public Guid? CreatedById { get; set; }

    /// <summary>
    /// Идентификатор назначенного исполнителя задачи.
    /// </summary>
    public Guid? AssignedToId { get; set; }

    /// <summary>
    /// Поле, по которому происходит сортировка.
    /// </summary>
    public string? SortBy { get; set; }

    /// <summary>
    /// Список связанных сущностей для включения их данных в ответ.
    /// </summary>
    public string? Includes { get; set; }
}