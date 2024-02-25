namespace Sevriukoff.ProjectManager.Application.Specification.Project;

public class IncludingSpecification<T> : Specification<T>
{
    public IncludingSpecification(params string[] includes)
    {
        foreach (var include in includes)
            AddInclude(include);
    }
}