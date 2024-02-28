using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Sevriukoff.ProjectManager.Application.Factories;
using Sevriukoff.ProjectManager.Application.Interfaces;
using Sevriukoff.ProjectManager.Application.Models;
using Sevriukoff.ProjectManager.Application.Services;
using Sevriukoff.ProjectManager.Application.Strategies.ProjectUpdateStrategy;
using Sevriukoff.ProjectManager.Application.Strategies.TaskUpdateStrategy;
using Sevriukoff.ProjectManager.Infrastructure;
using Sevriukoff.ProjectManager.Infrastructure.Authorization;
using Sevriukoff.ProjectManager.Infrastructure.Interfaces;
using Sevriukoff.ProjectManager.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

#region Dipendency Configurations

builder.Services.AddDbContext<ProjectDbContext>(
    opt => opt.UseSqlite(builder.Configuration.GetConnectionString("ProjDb")));

builder.Services.AddDbContext<AuthDbContext>(
        opt => opt.UseSqlite(builder.Configuration.GetConnectionString("AuthDb")))
    .AddIdentity<Employee, Role>(cfg =>
    {
        cfg.Password.RequireDigit = false;
        cfg.Password.RequireLowercase = false;
        cfg.Password.RequireNonAlphanumeric = false;
        cfg.Password.RequireUppercase = false;
        cfg.Password.RequiredLength = 9;
    })
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IProjectService, ProjectService>();

builder.Services.AddScoped<IProjectTaskRepository, ProjectTaskRepository>();
builder.Services.AddScoped<IProjectTaskService, ProjectTaskService>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthManager, AuthManager>();

builder.Services.AddTransient<ITaskUpdateStrategy, AdministratorTaskUpdateStrategy>();
builder.Services.AddTransient<ITaskUpdateStrategy, ManagerTaskUpdateStrategy>();
builder.Services.AddTransient<ITaskUpdateStrategy, EmployeeTaskUpdateStrategy>();

builder.Services.AddTransient<TaskUpdateStrategyFactory>();

builder.Services.AddTransient<IProjectUpdateStrategy, AdministratorProjectUpdateStrategy>();
builder.Services.AddTransient<IProjectUpdateStrategy, ManagerProjectUpdateStrategy>();
builder.Services.AddTransient<IProjectUpdateStrategy, EmployeeProjectUpdateStrategy>();

builder.Services.AddTransient<ProjectUpdateStrategyFactory>();

#endregion

#region Auth

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(180);

    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = 401;
        return Task.CompletedTask;
    };

    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = 401;
        return Task.CompletedTask;
    };
    
    options.SlidingExpiration = true;
});

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy(nameof(UserRole.Administrator), policyBuilder =>
    {
        policyBuilder.RequireClaim(ClaimTypes.Role, nameof(UserRole.Administrator));
    });

    opt.AddPolicy(nameof(UserRole.Manager), policyBuilder =>
    {
        policyBuilder.RequireAssertion(x => x.User.HasClaim(ClaimTypes.Role, nameof(UserRole.Manager))
                                            || x.User.HasClaim(ClaimTypes.Role, nameof(UserRole.Administrator)));
    });
    
    opt.AddPolicy(nameof(UserRole.Employee), policyBuilder =>
    {
        policyBuilder.RequireAssertion(x => x.User.HasClaim(ClaimTypes.Role, nameof(UserRole.Manager))
                                            || x.User.HasClaim(ClaimTypes.Role, nameof(UserRole.Administrator))
                                            || x.User.HasClaim(ClaimTypes.Role, nameof(UserRole.Employee)));
    });
});

#endregion

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Project manager API",
        Version = "v1",
        Description =
            "API для управления проектами, разработанный в рамках выполнения тестового задания от компании Sibers",
        Contact = new OpenApiContact
        {
            Name = "Igor Sevriukoff", 
            Url = new Uri("https://github.com/Sevriukoff")
        }
    });

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

app.UseAuthentication();

app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();