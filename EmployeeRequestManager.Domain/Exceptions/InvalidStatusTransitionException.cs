using EmployeeRequestManager.Domain.Enums;

namespace EmployeeRequestManager.Domain.Exeptions;

public class InvalidStatusTransitionException : Exception
{
    public InvalidStatusTransitionException(RequestStatus currentStatus, RequestStatus targetStatus) : base($"Can't change status from {currentStatus} to {targetStatus}")
    {
        CurrentStatus = currentStatus;
        TargetStatus = targetStatus;
    }
    
    public RequestStatus CurrentStatus { get; }
    public RequestStatus TargetStatus { get; }
}