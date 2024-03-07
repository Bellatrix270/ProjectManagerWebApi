﻿namespace Sevriukoff.ProjectManager.Application.Exception;

public class ValidationError
{
    public string Code { get; set; }
    
    public string Description { get; set; }
    
    public ValidationError(string code, string description)
    {
        Code = code;
        Description = description;
    }
}