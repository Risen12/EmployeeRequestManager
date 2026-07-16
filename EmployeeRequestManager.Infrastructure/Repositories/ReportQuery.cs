using EmployeeRequestManager.Application.Reports;
using EmployeeRequestManager.Domain.Enums;
using EmployeeRequestManager.Domain.Repositories;
using EmployeeRequestManager.Infrastructure.ValueConverters;
using Microsoft.EntityFrameworkCore;

namespace EmployeeRequestManager.Infrastructure.Repositories;

public class ReportQuery : IReportQuery
{
    private ApplicationDbContext _context;
    
    public ReportQuery(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ReportDto> GetReportAsync()
    {
        ReportDto report = new ReportDto();

        var converter = new StatusConverter();
        var toDbFunc = converter.ConvertToProviderExpression.Compile();
        
        var requestByStatus = _context.EmployeeRequests.GroupBy(e => e.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => toDbFunc(x.Status), x => x.Count);

        report.RequestsCountByStatus = await requestByStatus;

        var completedRequestsByExecutor = _context.EmployeeRequests.Where(r => r.Status == RequestStatus.Completed)
            .GroupBy(r => r.Executor.FullName).Select(g => new { Name = g.Key, Count = g.Count() }).ToDictionaryAsync(x => x.Name, x => x.Count);
        
        report.CompletedRequestsByExecutor = await completedRequestsByExecutor;

        var overdueRequestsCount = _context.EmployeeRequests.Count(r => r.Status == RequestStatus.InProgress || r.Status == RequestStatus.New && r.RequestExpirationDate < DateTime.Now);
        
        report.OverdueRequestsCount = overdueRequestsCount;
        
        return report;
    }
}