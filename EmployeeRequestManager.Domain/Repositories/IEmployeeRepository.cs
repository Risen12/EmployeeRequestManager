using EmployeeRequestManager.Domain.Entities;

namespace EmployeeRequestManager.Domain.Repositories;

public interface IEmployeeRepository
{
    public Task<Employee> GetEmployeeByIdAsync(int id);
    
    public Task<IReadOnlyList<Employee>> GetAllEmployeesAsync();
    
    public Task<Employee> CreateEmployeeAsync(Employee employee);
}