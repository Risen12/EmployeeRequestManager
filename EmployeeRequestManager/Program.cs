using EmployeeRequestManager.Application.Reports;
using EmployeeRequestManager.Application.Services;
using EmployeeRequestManager.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using EmployeeRequestManager.Infrastructure;
using EmployeeRequestManager.Infrastructure.Repositories;
using Scalar.AspNetCore;

namespace EmployeeRequestManager;

public class Program
{
    public static void Main(string[] args)
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
        app.MapControllers();
        app.MapOpenApi();
        app.MapScalarApiReference();
        app.UseExceptionHandler();

        app.UseStaticFiles();

        app.Run();
    }
}