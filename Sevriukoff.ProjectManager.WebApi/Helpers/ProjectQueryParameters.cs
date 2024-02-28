using System.ComponentModel.DataAnnotations;

namespace Sevriukoff.ProjectManager.WebApi.Helpers;

/// <summary>
/// Параметры запроса для проектов.
/// </summary>
public class ProjectQueryParameters
{
    /// <summary>
    /// Дата начала периода, в котором был запущен проект.
    /// </summary>
    [DataType(DataType.Date)]
    public DateTime? StartDateFrom { get; set; }

    /// <summary>
    /// Дата окончания периода, в котором был запущен проект.
    /// </summary>
    [DataType(DataType.Date)]
    public DateTime? StartDateTo { get; set; }

    /// <summary>
    /// Приоритет проекта от 1 до 10.
    /// </summary>
    [Range(1, 10)]
    public int? Priority { get; set; }

    /// <summary>
    /// Список связанных сущностей для включения их данных в ответ.
    /// </summary>
    public string? Includes { get; set; }

    /// <summary>
    /// Поле, по которому происходит сортировка.
    /// </summary>
    public string? SortBy { get; set; }
}