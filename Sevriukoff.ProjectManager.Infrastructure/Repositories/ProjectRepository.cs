using Microsoft.EntityFrameworkCore;
using Sevriukoff.ProjectManager.Infrastructure.Base;
using Sevriukoff.ProjectManager.Infrastructure.Entities;
using Sevriukoff.ProjectManager.Infrastructure.Repositories.Interfaces;

namespace Sevriukoff.ProjectManager.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly ProjectDbContext _context;

    public ProjectRepository(ProjectDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Project>> GetAllAsync(ISpecification<Project>? specification = null)
    {
        if (specification != null)
        {
            return await SpecificationEvaluator<Project>.GetQuery(_context.Set<Project>().AsQueryable(), specification)
                .ToListAsync();
        }
        
        return await _context.Projects.Include("Employees")
            .Include(x => x.Tasks)
            .ToListAsync();
    }

    public async Task<Project?> GetByIdAsync(int id, ISpecification<Project> specification = null)
        => await _context.Projects.Include(p => p.Employees)
            .Include(x => x.Tasks)
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<int> AddAsync(Project project)
    {
        await _context.Projects.AddAsync(project);
        await _context.SaveChangesAsync();

        return project.Id;
    }

    public async Task<bool> UpdateAsync(Project project)
    {
        _context.Projects.Update(project);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Project>> GetBySpecification(ISpecification<Project> specification)
    {
        var query = SpecificationEvaluator<Project>.GetQuery(_context.Set<Project>().AsQueryable(),
            specification);
        
        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Project>> GetByFilters(DateTime? startDateFrom, DateTime? startDateTo, int? priority)
    {
        var query = _context.Projects.AsQueryable();

        if (startDateFrom != null)
            query = query.Where(p => p.StartDate >= startDateFrom);

        if (startDateTo != null)
            query = query.Where(p => p.StartDate <= startDateTo);

        if (priority != null)
            query = query.Where(p => p.Priority == priority);

        return await query.Include(p => p.Manager)
            .Include(p => p.Employees)
            .ToListAsync();
    }

}