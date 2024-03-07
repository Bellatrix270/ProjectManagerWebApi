using System.ComponentModel;
using Microsoft.AspNetCore.Identity;
using Nelibur.ObjectMapper;
using Sevriukoff.ProjectManager.Application.Exception;
using Sevriukoff.ProjectManager.Application.Models;
using Sevriukoff.ProjectManager.Infrastructure.Authorization;
using Sevriukoff.ProjectManager.Infrastructure.Entities;

namespace Sevriukoff.ProjectManager.Application.Mapping;

public static class MapperWrapper
{
    private static bool _initialized;
    private static readonly object Lock = new();
    private static readonly MapperWrapperConfiguration WrapperConfiguration = new(); 
    
    private static readonly List<IMapperConfiguration> MapperConfigurations = new();

    public static void AddConfiguration(IMapperConfiguration mapperConfiguration)
    {
        MapperConfigurations.Add(mapperConfiguration);
    }

    private static void Initialize()
    {
        if (!_initialized)
        {
            lock (Lock)
            {
                if (!_initialized)
                {
                    WrapperConfiguration.Bind<Employee, EmployeeModel>();
                    WrapperConfiguration.Bind<EmployeeModel, Employee>();
                    
                    WrapperConfiguration.Bind<Project, ProjectModel>(c => c.Ignore(x => x.Employees));
                    WrapperConfiguration.Bind<ProjectModel, Project>();
                    
                    WrapperConfiguration.Bind<ProjectTask, ProjectTaskModel>();
                    WrapperConfiguration.Bind<ProjectTaskModel, ProjectTask>();
                    
                    WrapperConfiguration.Bind<ValidationError, IdentityError>();
                    WrapperConfiguration.Bind<IdentityError, ValidationError>();
                    
                    foreach (var configuration in MapperConfigurations)
                        configuration.Configure(WrapperConfiguration);

                    _initialized = true;
                }
            }
        }
    }

    public static TTarget? Map<TSource, TTarget>(TSource? source, TTarget target = default(TTarget))
        => (TTarget?)MapInternal(source, typeof(TTarget), typeof(TSource));

    public static object? Map(Type sourceType, Type targetType, object? source, object? target = null)
        => MapInternal(source, targetType, sourceType);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="source"></param>
    /// <typeparam name="TTarget"></typeparam>
    /// <returns></returns>
    /// <exception cref="NullReferenceException">If a source equals null</exception> 
    public static TTarget Map<TTarget>(object source)
        => (TTarget)MapInternal(source, typeof(TTarget), source.GetType())!;

    private static object? MapInternal(object? source, Type targetType, Type sourceType, object? target = null)
    {
        Initialize();
        
        return TinyMapper.Map(sourceType, targetType, source, target);
    }
}