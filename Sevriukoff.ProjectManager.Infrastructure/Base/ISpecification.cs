namespace Sevriukoff.ProjectManager.Infrastructure.Base;

public interface ISpecification<T>
{
    bool IsSatisfiedBy(T entity);
}