using EmployeeRequestManager.Domain.Entities;
using EmployeeRequestManager.Domain.Exeptions;
using EmployeeRequestManager.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EmployeeRequestManager.Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly ApplicationDbContext _dbContext;

    public EmployeeRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Employee> GetEmployeeByIdAsync(int id)
    {
        var employee = await _dbContext.Employees.FindAsync(id);

        if (employee == null)
            throw new NotFoundEmployeeException(id);
        
        return employee;
    }

    public async Task<IReadOnlyList<Employee>> GetAllEmployeesAsync()
    {
        var employees = await _dbContext.Employees.ToListAsync();
        
        return employees;
    }

    public async Task<Employee> CreateEmployeeAsync(Employee employee)
    {
        await _dbContext.Employees.AddAsync(employee);
        
        await _dbContext.SaveChangesAsync();
        return employee;
    }
}