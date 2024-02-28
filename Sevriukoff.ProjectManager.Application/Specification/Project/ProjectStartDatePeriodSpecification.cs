namespace Sevriukoff.ProjectManager.Application.Specification.Project;

public class ProjectStartDatePeriodSpecification : Specification<Infrastructure.Entities.Project>
{
    public ProjectStartDatePeriodSpecification(DateTime? startDateFrom, DateTime? startDateTo)
    {
        if (!startDateFrom.HasValue) return;

        if (startDateTo.HasValue)
            SetFilterCondition(p => p.StartDate >= startDateFrom && p.StartDate <= startDateTo);
        else
            SetFilterCondition(p => p.StartDate >= startDateFrom);
    }
}