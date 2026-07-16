using EmployeeRequestManager.Domain.Entities;

namespace EmployeeRequestManager.Domain.Repositories;

public interface IEmployeeRequestRepository
{
    public Task<EmployeeRequest> GetRequestByIdAsync(int id);
    
    public Task<EmployeeRequest> CreateRequestAsync(EmployeeRequest employeeRequest);
    
    public Task<EmployeeRequest> UpdateRequestAsync(EmployeeRequest employeeRequest);
    
    public Task<IReadOnlyList<EmployeeRequest>> GetAllRequestsAsync();

    public Task<IReadOnlyList<EmployeeRequest>> GetRequestByFilterAsync(RequestFilter filter);

}