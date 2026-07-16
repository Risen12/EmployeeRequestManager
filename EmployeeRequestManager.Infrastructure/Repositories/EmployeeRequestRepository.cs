using EmployeeRequestManager.Domain.Entities;
using EmployeeRequestManager.Domain.Exceptions;
using EmployeeRequestManager.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EmployeeRequestManager.Infrastructure.Repositories;

public class EmployeeRequestRepository : IEmployeeRequestRepository
{
    private ApplicationDbContext _context;
    
    public EmployeeRequestRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<EmployeeRequest> GetRequestByIdAsync(int id)
    {
        var request = await _context.Set<EmployeeRequest>().Include(r => r.Author)
            .Include(r => r.Executor)
            .FirstOrDefaultAsync(x => x.Id == id);

        return request == null ? throw new NotFoundRequestException(id) : request;
    }

    public async Task<EmployeeRequest> CreateRequestAsync(EmployeeRequest employeeRequest)
    {
        await _context.Set<EmployeeRequest>().AddAsync(employeeRequest);
        await _context.SaveChangesAsync();
        
        return employeeRequest;
    }

    public async Task<EmployeeRequest> UpdateRequestAsync(EmployeeRequest employeeRequest)
    {
        _context.Set<EmployeeRequest>().Update(employeeRequest);
        await _context.SaveChangesAsync();
        
        return employeeRequest;
    }

    public async Task<IReadOnlyList<EmployeeRequest>> GetAllRequestsAsync()
    {
        IReadOnlyList<EmployeeRequest> requests = await _context.Set<EmployeeRequest>()
            .Include(r => r.Executor)
            .Include(r => r.Author)
            .ToListAsync();
        
        return requests;
    }

    public async Task<IReadOnlyList<EmployeeRequest>> GetRequestByFilterAsync(RequestFilter filter)
    {
        var requests = _context.Set<EmployeeRequest>()
            .Include(r => r.Executor)
            .Include(r => r.Author)
            .AsQueryable();
        
        var department = filter.Department;
        var executorId = filter.ExecutorId;
        var status = filter.Status;

        if (department != null)
        { 
            requests = requests.Where(r => r.Author.Department == department);
        }

        if (executorId != null)
        {
            requests = requests.Where(r => r.ExecutorId == executorId);
        }

        if (status != null)
        {
            requests = requests.Where(r => r.Status == status);
        }

        return await requests.ToListAsync();
    }
}