using System.Linq.Expressions;

namespace Sevriukoff.ProjectManager.Infrastructure.Base;

public abstract class Specification<T> : ISpecification<T>
{
    public abstract Expression<Func<T, bool>> ToExpression();
    protected Expression<Func<T, bool>> Expression = null;
    
    public bool IsSatisfiedBy(T entity)
    {
        var predicate = ToExpression().Compile();
        return predicate(entity);
    }
}