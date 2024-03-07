using Sevriukoff.ProjectManager.Application.Exception;

namespace Sevriukoff.ProjectManager.Application.Models;

public class ProjectModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string CustomerCompany { get; set; }
    public string ExecutorCompany { get; set; }
    public DateTime StartDate  { get; set; }
    public DateTime EndDate  { get; set; }
    public int Priority { get; set; }

    public Guid ManagerId { get; set; }
    public List<EmployeeModel>? Employees { get; set; }
    public List<ProjectTaskModel>? Tasks { get; set; }
    
    public (bool IsValid, IEnumerable<ValidationError> Errors) IsValid()
    {
        var errors = new List<ValidationError>();

        if (string.IsNullOrWhiteSpace(Name))
            errors.Add(new ValidationError("NameRequired", "Название проекта обязательно"));

        if (string.IsNullOrWhiteSpace(CustomerCompany))
            errors.Add(new ValidationError("CustomerCompanyRequired", "Название компании-заказчика обязательно"));

        if (string.IsNullOrWhiteSpace(ExecutorCompany))
            errors.Add(new ValidationError("ExecutorCompanyRequired", "Название компании-исполнителя обязательно"));

        if (StartDate == default)
            errors.Add(new ValidationError("StartDateRequired", "Дата начала проекта обязательна"));

        if (EndDate == default)
            errors.Add(new ValidationError("EndDateRequired", "Дата окончания проекта обязательна"));

        if (StartDate > EndDate)
            errors.Add(new ValidationError("StartDateGreaterThanEndDate", "Дата начала задачи должна быть раньше даты окончания задачи"));

        if (Priority <= 0 || Priority > 10)
            errors.Add(new ValidationError("InvalidPriority", "Приоритет проекта должен быть в диапазоне от 1 до 10"));

        return (!errors.Any(), errors);
    }

}