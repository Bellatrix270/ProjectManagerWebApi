using System.Linq.Expressions;

namespace Sevriukoff.ProjectManager.Infrastructure.Base;

public class CompositeSpecification<T> : Specification<T>
{
    private readonly Specification<T> _left;
    private readonly Specification<T> _right;

    public CompositeSpecification(Specification<T> left, Specification<T> right)
    {
        _left = left;
        _right = right;
        
        var leftExpression = _left.ToExpression();
        var rightExpression = _right.ToExpression();

        var paramExpr = System.Linq.Expressions.Expression.Parameter(typeof(T));
        var exprBody = System.Linq.Expressions.Expression.AndAlso(leftExpression.Body, rightExpression.Body);
        exprBody = (BinaryExpression)new ParameterReplacer(paramExpr).Visit(exprBody);
        
        Expression = System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(exprBody, paramExpr);
    }
    
    public override Expression<Func<T, bool>> ToExpression()
    {
        return Expression;
    }
}

public class ParameterReplacer : ExpressionVisitor
{
    private readonly ParameterExpression _parameter;

    protected override Expression VisitParameter(ParameterExpression node)
        => base.VisitParameter(_parameter);

    internal ParameterReplacer(ParameterExpression parameter)
    {
        _parameter = parameter;
    }
}