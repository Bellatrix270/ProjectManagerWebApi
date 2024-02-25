using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Sevriukoff.ProjectManager.Application.Interfaces;
using Sevriukoff.ProjectManager.Application.Services;
using Sevriukoff.ProjectManager.Infrastructure;
using Sevriukoff.ProjectManager.Infrastructure.Repositories;
using Sevriukoff.ProjectManager.Infrastructure.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

#region Dipendency Configurations

builder.Services.AddDbContext<ProjectDbContext>(
    opt => opt.UseSqlite(builder.Configuration.GetConnectionString("ProjDb")));

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IProjectService, ProjectService>();

builder.Services.AddScoped<IProjectTaskRepository, ProjectTaskRepository>();
builder.Services.AddScoped<IProjectTaskService, ProjectTaskService>();

#endregion

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.UseInlineDefinitionsForEnums();
    
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Project manager API", Version = "v1", Description = "123"});

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();