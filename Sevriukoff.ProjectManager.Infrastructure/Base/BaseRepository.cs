using Microsoft.EntityFrameworkCore;
using Sevriukoff.ProjectManager.Infrastructure.Interfaces;

namespace Sevriukoff.ProjectManager.Infrastructure.Base;

public abstract class BaseRepository<T, TId, TContext> : IRepository<T, TId>
    where T : class, IBaseEntity<TId> where TContext : DbContext
{
    protected readonly TContext Context;

    protected BaseRepository(TContext context)
    {
        Context = context;
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(ISpecification<T>? specification = null)
    {
        if (specification != null)
            return await SpecificationEvaluator<T, TId>.GetQuery(Context.Set<T>().AsQueryable(), specification)
                .ToListAsync();

        return await Context.Set<T>().ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync(TId id)
        => await Context.Set<T>().FindAsync(id);

    public async Task<TId> AddAsync(T entity)
    {
        await Context.Set<T>().AddAsync(entity);
        await Context.SaveChangesAsync();

        return entity.Id;
    }

    public async Task<bool> UpdateAsync(T entity)
    {
        var existingEntity = await Context.Set<T>().FindAsync(entity.Id);
        
        if (existingEntity == null)
            return false;
        
        Context.Entry(existingEntity).CurrentValues.SetValues(entity);
        return await Context.SaveChangesAsync() > 0;
    }


    public async Task<bool> DeleteAsync(TId id)
    {
        var existingEntity = await Context.Set<T>().FindAsync(id);
        
        if (existingEntity == null)
            return false;

        Context.Set<T>().Remove(existingEntity);
        return await Context.SaveChangesAsync() > 0;
    }
}