using EmployeeRequestManager.Domain.Enums;
using EmployeeRequestManager.Domain.Exceptions;

namespace EmployeeRequestManager.Domain.Entities;

public class EmployeeRequest
{
    public int Id { get; set; }
    public DateTime RequestCreationDate { get; set; }
    public Employee Author { get; set; }
    public Employee Executor { get; set; }
    public string Description { get; set; }
    public DateTime RequestExpirationDate { get; set; }
    public RequestStatus Status { get;  set; }
    public int AuthorId { get; set; }
    public int ExecutorId { get; set; }

    public void ChangeStatus(RequestStatus newStatus)
    {
        if ((Status == RequestStatus.New && newStatus != RequestStatus.InProgress)
            || (Status == RequestStatus.InProgress && newStatus != RequestStatus.Completed)
            || Status == RequestStatus.Completed)
        {
            throw new InvalidStatusTransitionException(Status, newStatus);
        }

        Status = newStatus;
    }

    public void ChangeExecutor(Employee newExecutor)
    {
        Executor = newExecutor;
        ExecutorId = newExecutor.Id;
    }
}