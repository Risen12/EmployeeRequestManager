using EmployeeRequestManager.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeRequestManager.Controllers;

[ApiController]
[Route("[controller]")]
public class ReportsController
{
    private readonly IReportQuery _reportQuery;

    public ReportsController(IReportQuery reportQuery)
    {
        _reportQuery = reportQuery;
    }
}