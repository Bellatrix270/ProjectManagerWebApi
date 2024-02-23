using System.ComponentModel;
using System.Globalization;
using Sevriukoff.ProjectManager.Application.Models;
using Sevriukoff.ProjectManager.Infrastructure.Entities;

namespace Sevriukoff.ProjectManager.Application.Mapping;

public class EmployeeClassConverter : TypeConverter
{
    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
    {
        return destinationType == typeof(EmployeeModel);
    }

    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
        var concreteValue = (Employee)value;
        var result = new EmployeeModel
        {
            Id = concreteValue.Id,
            FirstName = concreteValue.FirstName,
            LastName = concreteValue.LastName,
            Patronymic = concreteValue.Patronymic,
            FullName = $"{concreteValue.FirstName} {concreteValue.LastName} {concreteValue.Patronymic}",
            Email = concreteValue.Email
        };
        return result;
    }
}