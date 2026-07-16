using EmployeeRequestManager.Domain.Entities;

namespace EmployeeRequestManager.Domain.Repositories;

public interface IReportQuery
{
    Task<ReportDto> GetReportAsync();
}