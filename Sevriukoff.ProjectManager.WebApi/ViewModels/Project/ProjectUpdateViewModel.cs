#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Sevriukoff.ProjectManager.WebApi.ViewModels.Project;

public class ProjectUpdateViewModel : ProjectCreateViewModel
{
    public Guid Id { get; set; }
}