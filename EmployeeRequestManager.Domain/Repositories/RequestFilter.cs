using EmployeeRequestManager.Domain.Entities;
using EmployeeRequestManager.Domain.Enums;

namespace EmployeeRequestManager.Domain.Repositories;

public class RequestFilter
{
    public RequestStatus? Status { get; set; }
    public int? ExecutorId { get; set; }
    public string? Department { get; set; }
    public bool? IsOverdue { get; set; }
}