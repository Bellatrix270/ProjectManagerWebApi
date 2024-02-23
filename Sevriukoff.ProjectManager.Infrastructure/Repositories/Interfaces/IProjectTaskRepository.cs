﻿using Sevriukoff.ProjectManager.Infrastructure.Entities;

namespace Sevriukoff.ProjectManager.Infrastructure.Repositories.Interfaces;

public interface IProjectTaskRepository
{
    Task<IEnumerable<ProjectTask>> GetAllAsync();
    Task<ProjectTask?> GetByIdAsync(int id);
    Task<int> AddAsync(ProjectTask employee);
    Task<bool> UpdateAsync(ProjectTask employee);
    Task<bool> DeleteAsync(int id);
}