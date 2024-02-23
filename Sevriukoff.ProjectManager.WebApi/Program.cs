using Microsoft.EntityFrameworkCore;
using Sevriukoff.ProjectManager.Application.Interfaces;
using Sevriukoff.ProjectManager.Application.Services;
using Sevriukoff.ProjectManager.Infrastructure;
using Sevriukoff.ProjectManager.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

#region Dipendency Configurations

builder.Services.AddDbContext<ProjectDbContext>(
    opt => opt.UseSqlite(builder.Configuration.GetConnectionString("ProjDb")));

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IProjectService, ProjectService>();

#endregion

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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