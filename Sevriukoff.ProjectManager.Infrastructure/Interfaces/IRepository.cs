using Sevriukoff.ProjectManager.Infrastructure.Base;

namespace Sevriukoff.ProjectManager.Infrastructure.Interfaces;

public interface IRepository<T, TId> where T : IBaseEntity<TId>
{
    Task<IEnumerable<T>> GetAllAsync(ISpecification<T>? specification = null);
    Task<T?> GetByIdAsync(TId id);
    Task<TId> AddAsync(T entity);
    Task<bool> UpdateAsync(T entity);
    Task<bool> DeleteAsync(TId id);
}