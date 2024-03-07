using Nelibur.ObjectMapper;
using Nelibur.ObjectMapper.Bindings;

namespace Sevriukoff.ProjectManager.Application.Mapping;

public class MapperWrapperConfiguration
{
    public void Bind<TSource, TTarget>()
    {
        TinyMapper.Bind<TSource, TTarget>();
    }

    public void Bind(Type sourceType, Type targetType)
    {
        TinyMapper.Bind(sourceType, targetType);
    }

    public void Bind<TSource, TTarget>(Action<IBindingConfig<TSource, TTarget>> config)
    {
        TinyMapper.Bind(config);
    }
}