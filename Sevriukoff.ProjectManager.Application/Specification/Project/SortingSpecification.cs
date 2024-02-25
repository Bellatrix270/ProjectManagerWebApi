﻿using Sevriukoff.ProjectManager.Application.Exception;

namespace Sevriukoff.ProjectManager.Application.Specification.Project;

public class SortingSpecification<T> : Specification<T>
{
    public SortingSpecification(string sortBy)
    {
        const string desc = "Desc";
        var propertyPath = sortBy;
        bool descending = false;
        
        if (propertyPath.EndsWith(desc, StringComparison.OrdinalIgnoreCase))
        {
            propertyPath = sortBy.Substring(0, sortBy.Length - desc.Length).TrimEnd();
            descending = true;
        }

        if (!PropertyExists(propertyPath))
            throw new SpecificationException("Prop does not exist");
        
        ApplySort(propertyPath, descending);
    }
    
    private bool PropertyExists(string propertyName)
    {
        return typeof(T).GetProperties().Any(prop => prop.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));
    }
}