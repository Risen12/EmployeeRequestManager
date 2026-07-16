using EmployeeRequestManager.Application.Reports;
using EmployeeRequestManager.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeRequestManager.Controllers;

[ApiController]
[Route("[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReportQuery _reportQuery;

    public ReportsController(IReportQuery reportQuery)
    {
        _reportQuery = reportQuery;
    }

    [HttpGet("OverdueRequests")]
    public async Task<int> GetOverdueRequests()
    {
        var reports = await _reportQuery.GetReportAsync();

        return reports.OverdueRequestsCount;
    }

    [HttpGet("RequestByStatus")]
    public async Task<Dictionary<string, int>> GetRequestsByStatusAsync()
    {
        var reports = await _reportQuery.GetReportAsync();

        return reports.RequestsCountByStatus;
    }

    [HttpGet("CompletedRequestsByExecutor")]
    public async Task<Dictionary<string, int>> GetCompletedRequestsByExecutorAsync()
    {
        var reports = await _reportQuery.GetReportAsync();

        return reports.CompletedRequestsByExecutor;
    }
}