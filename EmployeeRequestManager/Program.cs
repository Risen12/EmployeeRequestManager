using EmployeeRequestManager.Application.Services;
using EmployeeRequestManager.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using EmployeeRequestManager.Infrastructure;
using EmployeeRequestManager.Infrastructure.Repositories;

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
        builder.Services.AddControllers();
        
        var app = builder.Build();
        app.MapControllers();

        app.UseStaticFiles();

        app.MapGet("/", () => "Hello World!");

        app.Run();
    }
}