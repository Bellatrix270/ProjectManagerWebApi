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

    public async Task<IEnumerable<Project>> GetAllAsync()
        => await _context.Projects.Include(p => p.Employees)
            .Include(x => x.Tasks)
            .ToListAsync();

    public async Task<Project?> GetByIdAsync(int id)
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

    public IEnumerable<Project> GetBySpecification(Specification<Project> specification)
    {
        return _context.Projects.Where(specification.ToExpression()).ToList();
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