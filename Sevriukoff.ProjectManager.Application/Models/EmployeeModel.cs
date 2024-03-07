using System.Text.RegularExpressions;
using Sevriukoff.ProjectManager.Application.Exception;

namespace Sevriukoff.ProjectManager.Application.Models;

public class EmployeeModel
{
    public Guid? Id { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Patronymic { get; set; }
    public string Email { get; set; }
    public UserRole Role { get; set; }
    
    public (bool IsValid, IEnumerable<ValidationError> Errors) IsValid()
    {
        var errors = new List<ValidationError>();

        if (string.IsNullOrWhiteSpace(UserName))
            errors.Add(new ValidationError("UsernameRequired", "Имя пользователя обязательно"));

        if (!IsValidName(FirstName))
            errors.Add(new ValidationError("FirstNameCanOnlyContainLetters", "Имя сотрудника обязательно и должно содержать только буквы из русского алфавита"));

        if (!IsValidName(LastName))
            errors.Add(new ValidationError("LastNameCanOnlyContainLetters", "Фамилия сотрудника обязательно и должно содержать только буквы из русского алфавита"));

        if (!IsValidName(Patronymic))
            errors.Add(new ValidationError("PatronymicCanOnlyContainLetters", "Отчество сотрудника обязательно и должно содержать только буквы из русского алфавита"));

        if (string.IsNullOrWhiteSpace(Email))
            errors.Add(new ValidationError("EmailRequired", "Почта обязательна"));
        
        if (!IsValidEmail(Email))
            errors.Add(new ValidationError("InvalidEmail", "Почта имеет неверный формат"));
        
        if (Role < 0 || (int)Role >= Enum.GetValues(typeof(UserRole)).Length)
            errors.Add(new ValidationError("InvalidRole", "Недопустимая роль пользователя"));
        
        //TODO: Added validation for managerId

        return (!errors.Any(), errors);
    }
    
    private bool IsValidName(string name)
        => !string.IsNullOrWhiteSpace(name) && Regex.IsMatch(name, @"^[a-zA-Z]+$");

    private bool IsValidEmail(string email)
        => !string.IsNullOrWhiteSpace(email) && Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
}

public enum UserRole
{
    Employee,
    Manager,
    Administrator
}

