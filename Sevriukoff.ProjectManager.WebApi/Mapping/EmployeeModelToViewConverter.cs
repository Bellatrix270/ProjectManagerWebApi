using System.ComponentModel;
using System.Globalization;
using Sevriukoff.ProjectManager.Application.Models;
using Sevriukoff.ProjectManager.WebApi.ViewModels.Employee;
#pragma warning disable CS1591

namespace Sevriukoff.ProjectManager.WebApi.Mapping;

public class EmployeeModelToViewConverter : TypeConverter
{
    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
    {
        return destinationType == typeof(EmployeeViewModel);
    }

    public override object ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value,
        Type destinationType)
    {
        if (value == null)
            throw new NullReferenceException();
        
        var concreteValue = (EmployeeModel)value;
        var result = new EmployeeViewModel
        {
            Id = concreteValue.Id!.Value,
            UserName = concreteValue.UserName,
            FullName = $"{concreteValue.FirstName} {concreteValue.LastName} {concreteValue.Patronymic}",
            Email = concreteValue.Email,
            Role = concreteValue.Role
        };
        
        return result;
    }
}