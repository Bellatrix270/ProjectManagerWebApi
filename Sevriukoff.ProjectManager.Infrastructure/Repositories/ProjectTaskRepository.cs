using Microsoft.EntityFrameworkCore;
using Sevriukoff.ProjectManager.Infrastructure.Base;
using Sevriukoff.ProjectManager.Infrastructure.Entities;
using Sevriukoff.ProjectManager.Infrastructure.Interfaces;

namespace Sevriukoff.ProjectManager.Infrastructure.Repositories;

public class ProjectTaskRepository : BaseRepository<ProjectTask, Guid, ProjectDbContext>, IProjectTaskRepository
{
    public ProjectTaskRepository(ProjectDbContext context) : base(context) { }
    
    public async Task<IEnumerable<ProjectTask>> GetBySpecificationAsync(ISpecification<ProjectTask> specification)
    {
        var query = SpecificationEvaluator<ProjectTask, Guid>.GetQuery(Context.Set<ProjectTask>()
                .AsQueryable(), specification);
        
        return await query.ToListAsync();
    }
}