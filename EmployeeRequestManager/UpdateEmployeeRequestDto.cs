using EmployeeRequestManager.Domain.Enums;

namespace EmployeeRequestManager.Application.DTO;

public class UpdateEmployeeRequestDto
{
    public int? NewExecutorId { get; set; }
    public RequestStatus? NewStatus { get; set; }
}