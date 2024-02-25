namespace Sevriukoff.ProjectManager.Application.Specification.Project;

public class TimePeriodSpecification : Specification<Infrastructure.Entities.Project>
{
    public TimePeriodSpecification(DateTime? startDateFrom, DateTime? startDateTo)
    {
        if (!startDateFrom.HasValue) return;

        if (startDateTo.HasValue)
            SetFilterCondition(p => p.StartDate >= startDateFrom && p.StartDate <= startDateTo);
        else
            SetFilterCondition(p => p.StartDate >= startDateFrom);
    }
}