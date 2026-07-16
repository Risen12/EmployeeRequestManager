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
        builder.Services.AddSingleton<EmployeeRequestService>();
        builder.Services.AddSingleton<IReportQuery, ReportQuery>();
        
        var app = builder.Build();

        app.UseStaticFiles();

        app.MapGet("/", () => "Hello World!");

        app.Run();
    }
}