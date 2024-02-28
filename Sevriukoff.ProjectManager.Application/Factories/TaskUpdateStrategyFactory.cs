using Microsoft.Extensions.DependencyInjection;
using Sevriukoff.ProjectManager.Application.Models;
using Sevriukoff.ProjectManager.Application.Strategies.TaskUpdateStrategy;
using Sevriukoff.ProjectManager.Infrastructure.Interfaces;

namespace Sevriukoff.ProjectManager.Application.Factories;

public interface ITaskUpdateStrategyFactory
{
    ITaskUpdateStrategy CreateStrategy(UserContext userContext);
}

/*public class TaskUpdateStrategyFactory : ITaskUpdateStrategyFactory
{
    private readonly IServiceProvider _serviceProvider;

    public TaskUpdateStrategyFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ITaskUpdateStrategy CreateStrategy(UserContext userContext)
    {
        switch (userContext.Role)
        {
            case UserRole.Administrator:
                return _serviceProvider.GetRequiredService<AdministratorTaskUpdateStrategy>();
            case UserRole.Manager:
                return _serviceProvider.GetRequiredService<ProjectManagerTaskUpdateStrategy>();
            case UserRole.Employee:
                return _serviceProvider.GetRequiredService<EmployeeTaskUpdateStrategy>();
            default:
                throw new InvalidOperationException("Неизвестная роль пользователя.");
        }
    }
}*/

public class TaskUpdateStrategyFactory
{
    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly IProjectRepository _projectRepository;

    public TaskUpdateStrategyFactory(IProjectTaskRepository projectTaskRepository, IProjectRepository projectRepository)
    {
        _projectTaskRepository = projectTaskRepository;
        _projectRepository = projectRepository;
    }

    public ITaskUpdateStrategy CreateStrategy(UserContext userContext)
    {
        return userContext.Role switch
        {
            UserRole.Administrator => new AdministratorTaskUpdateStrategy(_projectTaskRepository),
            UserRole.Manager => new ManagerTaskUpdateStrategy(_projectTaskRepository, _projectRepository),
            UserRole.Employee => new EmployeeTaskUpdateStrategy(_projectTaskRepository),
            _ => throw new InvalidOperationException("Неизвестная роль пользователя.")
        };
    }
}