using System.ComponentModel;
using Nelibur.ObjectMapper;
using Sevriukoff.ProjectManager.Application.Models;
using Sevriukoff.ProjectManager.Infrastructure.Entities;

namespace Sevriukoff.ProjectManager.Application.Mapping;

public static class MapperWrapper
{
    private static bool _initialized = false;
    private static readonly object _lock = new object();

    private static void Initialize()
    {
        if (!_initialized)
        {
            lock (_lock)
            {
                if (!_initialized)
                {
                    TypeDescriptor.AddAttributes(typeof(Employee),
                        new TypeConverterAttribute(typeof(EmployeeClassConverter)));
                    
                    TinyMapper.Bind<Employee, EmployeeModel>();
                    TinyMapper.Bind<EmployeeModel, Employee>();
                    TinyMapper.Bind<Project, ProjectModel>();
                    TinyMapper.Bind<ProjectModel, Project>();

                    _initialized = true;
                }
            }
        }
    }

    public static TTarget Map<TSource, TTarget>(TSource source, TTarget target = default(TTarget))
        => (TTarget)MapInternal(source, typeof(TTarget), source.GetType());

    public static object Map(Type sourceType, Type targetType, object source, object target = null)
        => MapInternal(source, targetType, sourceType);

    public static TTarget Map<TTarget>(object source)
        => (TTarget)MapInternal(source, typeof(TTarget), source.GetType());

    private static object MapInternal(object source, Type targetType, Type sourceType, object target = null)
    {
        Initialize();
        
        return TinyMapper.Map(sourceType, targetType, source, target);
    }
}