﻿namespace Sevriukoff.ProjectManager.Application.Models;

public class EmployeeModel
{
    public int? Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Patronymic { get; set; }
    public string? FullName { get; set; }
    public string Email { get; set; }
}
