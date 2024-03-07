using Sevriukoff.ProjectManager.Application.Exception;
using Sevriukoff.ProjectManager.Infrastructure.Entities;

namespace Sevriukoff.ProjectManager.Application.Models;

public class ProjectTaskModel
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public string Name { get; init; }
    public Guid CreatedById { get; set; }
    public EmployeeModel? CreatedBy { get; set; }
    public Guid? AssignedToId { get; set; }
    public EmployeeModel? AssignedTo { get; set; }
    public ProjectTaskStatus Status { get; set; }
    public string? Comment { get; set; }
    public int Priority { get; set; }
    
    public (bool IsValid, IEnumerable<ValidationError> Errors) IsValid()
    {
        var errors = new List<ValidationError>();

        if (string.IsNullOrWhiteSpace(Name))
            errors.Add(new ValidationError("NameRequired", "Название задачи обязательно"));

        if (ProjectId == Guid.Empty)
            errors.Add(new ValidationError("ProjectIdRequired", "Идентификатор проекта обязателен"));

        if (CreatedById == Guid.Empty)
            errors.Add(new ValidationError("CreatedByIdRequired", "Идентификатор создателя задачи обязателен"));

        if (Status < 0 || (int)Status >= Enum.GetValues(typeof(ProjectTaskStatus)).Length)
            errors.Add(new ValidationError("InvalidStatus", "Недопустимый статус задачи"));

        if (Priority <= 0 || Priority > 10)
            errors.Add(new ValidationError("InvalidPriority", "Приоритет задачи должен быть в диапазоне от 1 до 10"));

        return (!errors.Any(), errors);
    }
}