using EmployeeRequestManager.Domain.Repositories;

namespace EmployeeRequestManager.Application.Reports;

public interface IReportQuery
{
    Task<ReportDto> GetReportAsync();
}