using EmployeeRequestManager.Domain.Entities;
using EmployeeRequestManager.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace EmployeeRequestManager.Infrastructure;

public sealed class ApplicationDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; } = null!;
    public DbSet<EmployeeRequest> EmployeeRequests { get; set; } = null!;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        EmployeeRequestConfiguration employeeRequestConfiguration = new EmployeeRequestConfiguration();
        EmployeeConfiguration employeeConfiguration = new EmployeeConfiguration();
        
        modelBuilder.ApplyConfiguration(employeeConfiguration);
        modelBuilder.ApplyConfiguration(employeeRequestConfiguration);
    }
}