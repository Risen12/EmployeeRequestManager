using EmployeeRequestManager.Application.Reports;
using EmployeeRequestManager.Application.Services;
using EmployeeRequestManager.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using EmployeeRequestManager.Infrastructure;
using EmployeeRequestManager.Infrastructure.Repositories;
using EmployeeRequestManager.Infrastructure.Seeding;
using Scalar.AspNetCore;

namespace EmployeeRequestManager;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        var connection = builder.Configuration.GetConnectionString("DefaultConnection");
        
        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connection));
        builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        builder.Services.AddScoped<IEmployeeRequestRepository, EmployeeRequestRepository>();
        builder.Services.AddScoped<EmployeeRequestService>();
        builder.Services.AddScoped<IReportQuery, ReportQuery>();
        builder.Services.AddOpenApi();
        builder.Services.AddControllers();
        builder.Services.AddProblemDetails();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        
        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var seeder = new DataBaseSeeder();

            await seeder.SeedAsync(db);
        }

        app.UseDefaultFiles();
        app.UseStaticFiles();
        app.UseExceptionHandler();
        app.MapControllers();
        app.MapOpenApi();
        app.MapScalarApiReference();
        
        app.Run();
    }
}